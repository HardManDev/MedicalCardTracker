// Copyright (c) 2023 Mikulchik Vladislav Alekseevich <hardman.dev@pm.me>.
// This software is licensed under the MIT license.
// Please see the LICENSE file for more information.

using MedicalCardTracker.Application.Server.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MedicalCardTracker.Database;

public static class DependencyInjection
{
    public static IServiceCollection AddDatabase(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlite(configuration["DbConnectionUrl"]));
        services.AddScoped<IApplicationDbContext>(provider =>
            provider.GetService<ApplicationDbContext>()!);
        return services;
    }
}
