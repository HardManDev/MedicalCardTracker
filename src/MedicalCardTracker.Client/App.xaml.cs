// Copyright (c) 2023 Mikulchik Vladislav Alekseevich <hardman.dev@pm.me>.
// This software is licensed under the MIT license.
// Please see the LICENSE file for more information.

using System;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Windows;
using MedicalCardTracker.Application;
using MedicalCardTracker.Application.Client.Configuration;
using MedicalCardTracker.Application.Client.Requests;
using MedicalCardTracker.Application.Logging;
using MedicalCardTracker.Client.ViewModels;
using MedicalCardTracker.Client.Views;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace MedicalCardTracker.Client;

public partial class App : System.Windows.Application
{
    private readonly IServiceProvider _serviceProvider;

    public App() =>
        _serviceProvider = ConfigureServices().BuildServiceProvider();

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
        services.AddSingleton(
            new HubConnectionBuilder()
                .WithUrl($"{configuration.ApiBaseUrl}/notifications")
                .Build());
        services.AddSingleton<CustomerView>();
        services.AddSingleton<CustomerViewModel>();

        return services;
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        var configuration = _serviceProvider.GetRequiredService<ApplicationConfiguration>();

        if (configuration.IsWriteLog)
            Log.Logger = Assembly.GetExecutingAssembly().GetLogger();

        Log.Information("Application has been started...");

#if DEBUG
        if (!configuration.IsRegistrar)
            _serviceProvider.GetRequiredService<CustomerView>()
                .Show();
#endif

        base.OnStartup(e);
    }
}
