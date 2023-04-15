// Copyright (c) 2023 Mikulchik Vladislav Alekseevich <hardman.dev@pm.me>.
// This software is licensed under the MIT license.
// Please see the LICENSE file for more information.

using System;
using System.Windows;
using MedicalCardTracker.Application;
using MedicalCardTracker.Application.Client.Requests;
using Microsoft.Extensions.DependencyInjection;

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

        return services;
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
    }
}
