// Copyright (c) 2023 Mikulchik Vladislav Alekseevich <hardman.dev@pm.me>.
// This software is licensed under the MIT license.
// Please see the LICENSE file for more information.

using System;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Markup;
using MedicalCardTracker.Application;
using MedicalCardTracker.Application.Client.Configuration;
using MedicalCardTracker.Application.Client.Requests;
using MedicalCardTracker.Application.Logging;
using MedicalCardTracker.Client.Models.Enums;
using MedicalCardTracker.Client.Utils;
using MedicalCardTracker.Client.ViewModels;
using MedicalCardTracker.Client.Views;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace MedicalCardTracker.Client;

public partial class App : System.Windows.Application
{
#if !DEBUG
    private MutexHelper? _mutexHelper;
#endif
    private readonly IServiceProvider _serviceProvider;

    public App()
    {
        try
        {
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("ru-RU");
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("ru-RU");

            FrameworkElement.LanguageProperty.OverrideMetadata(
                typeof(FrameworkElement),
                new FrameworkPropertyMetadata(
                    XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));
        }
        catch (Exception)
        {
            // ignore
        }

        _serviceProvider = ConfigureServices().BuildServiceProvider();
    }

    private static IServiceCollection ConfigureServices(IServiceCollection? services = null)
    {
        var configuration = new ApplicationConfiguration(Directory.GetCurrentDirectory());

        services ??= new ServiceCollection();

        services.AddApplication();
        services.AddMediatR(config =>
            config.RegisterServicesFromAssembly(typeof(BaseRequestHandler).Assembly));
        services.AddSingleton(new HttpClient
        {
            BaseAddress = new Uri(configuration.ApiBaseUrl)
        });
        services.AddSingleton(configuration);
        services.AddSingleton<HubConnectionHelper>();
        services.AddSingleton<CustomerView>();
        services.AddSingleton<CustomerViewModel>();
        services.AddSingleton<HubConnectingView>();
        services.AddSingleton<HubConnectingViewModel>();
        services.AddSingleton<CardRequestsView>();
        services.AddSingleton<CardRequestsViewModel>();
        services.AddSingleton<TaskbarIconView>();
        services.AddSingleton<TaskbarIconViewModel>();
        services.AddSingleton<ConfigurationView>();
        services.AddSingleton<ConfigurationViewModel>();

        return services;
    }

    protected override async void OnStartup(StartupEventArgs e)
    {
        var configuration = _serviceProvider.GetRequiredService<ApplicationConfiguration>();
        var customerView = _serviceProvider.GetRequiredService<CustomerView>();
        var cardRequestsView = _serviceProvider.GetRequiredService<CardRequestsView>();
        var taskbarIconView = _serviceProvider.GetRequiredService<TaskbarIconView>();
        var hubConnectionHelper = _serviceProvider.GetRequiredService<HubConnectionHelper>();
        var hubConnectingView = _serviceProvider.GetRequiredService<HubConnectingView>();

#if !DEBUG
        _mutexHelper = new MutexHelper("MedicalCardTracker.Client", "MedicalCardTracker.Client-Pipe");
        _mutexHelper.DuplicateInstanceStartup += (sender, args) =>
        {
            Current.Dispatcher.Invoke(() =>
            {
                if (configuration.IsRegistrar)
                {
                    cardRequestsView.Show();
                    cardRequestsView.Topmost = true;
                    cardRequestsView.Topmost = false;
                }
                else
                {
                    customerView.Show();
                    customerView.Topmost = true;
                    customerView.Topmost = false;
                }
            });
        };
#endif

        if (configuration.IsWriteLog)
            Log.Logger = Assembly.GetExecutingAssembly().GetLogger();

        Log.Information("Application has been started...");

        taskbarIconView.Show();
        taskbarIconView.Hide();

        hubConnectingView.Show();

        hubConnectionHelper.PropertyChanged += (sender, args) =>
        {
            if (args.PropertyName == nameof(hubConnectionHelper.HubConnectionStatus))
                switch (hubConnectionHelper.HubConnectionStatus)
                {
                    case HubConnectionStatus.Reconnecting:
                        hubConnectingView.Show();
                        break;
                    case HubConnectionStatus.Disconnected:
                        hubConnectingView.Show();
                        break;
                    case HubConnectionStatus.Failed:
                        hubConnectingView.Hide();
                        break;
                    case HubConnectionStatus.Connecting:
                        hubConnectingView.Show();
                        break;
                    case HubConnectionStatus.Connected:
                        hubConnectingView.Hide();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
        };

        await hubConnectionHelper.ConnectToNotificationHub();

#if DEBUG
        if (!configuration.IsRegistrar)
            customerView.Show();
        else
            cardRequestsView.Show();
#endif

        base.OnStartup(e);
    }

    protected override void OnExit(ExitEventArgs e)
    {
#if !DEBUG
        _mutexHelper?.Dispose();
#endif
        base.OnExit(e);
    }
}
