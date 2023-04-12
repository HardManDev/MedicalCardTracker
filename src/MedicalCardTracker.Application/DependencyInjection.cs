// Copyright (c) 2023 Mikulchik Vladislav Alekseevich <hardman.dev@pm.me>.
// This software is licensed under the MIT license.
// Please see the LICENSE file for more information.

using System.Reflection;
using MedicalCardTracker.Application.Interfaces;
using MedicalCardTracker.Application.Mappings;
using Microsoft.Extensions.DependencyInjection;

namespace MedicalCardTracker.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(config =>
        {
            config.AddProfile(new AssemblyMappingProfile(Assembly.GetExecutingAssembly()));
            config.AddProfile(new AssemblyMappingProfile(typeof(IMapWith<>).Assembly));
        });
        services.AddMediatR(config =>
            config.RegisterServicesFromAssembly(Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly()));

        return services;
    }
}
