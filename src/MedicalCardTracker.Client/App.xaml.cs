// Copyright (c) 2023 Mikulchik Vladislav Alekseevich <hardman.dev@pm.me>.
// This software is licensed under the MIT license.
// Please see the LICENSE file for more information.

using System;
using System.IO;
using System.Reflection;
using System.Windows;
using MedicalCardTracker.Application;
using MedicalCardTracker.Application.Client.Configuration;
using MedicalCardTracker.Application.Client.Requests;
using MedicalCardTracker.Application.Logging;
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
        services ??= new ServiceCollection();

        services.AddApplication();
        services.AddMediatR(config =>
            config.RegisterServicesFromAssembly(typeof(BaseRequestHandler).Assembly));
        services.AddSingleton(
            new ApplicationConfiguration(Directory.GetCurrentDirectory()));

        return services;
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        var configuration = _serviceProvider.GetRequiredService<ApplicationConfiguration>();

        if (configuration.IsWriteLog)
            Log.Logger = Assembly.GetExecutingAssembly().GetLogger();

        base.OnStartup(e);
    }
}
