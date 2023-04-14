// Copyright (c) 2023 Mikulchik Vladislav Alekseevich <hardman.dev@pm.me>.
// This software is licensed under the MIT license.
// Please see the LICENSE file for more information.

using MedicalCardTracker.Database;
using MedicalCardTracker.Server;
using MedicalCardTracker.Tests.Database;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace MedicalCardTracker.Client.Tests.Requests;

[Collection("CardRequestCollection")]
public abstract class BaseRequestHandler
{
    protected BaseRequestHandler(WebApplicationFactory<Program> fixture)
        => HttpClient = fixture
            .WithWebHostBuilder(builder =>
            {
                builder.UseEnvironment("Testing");
                builder.ConfigureServices(services =>
                {
                    var serviceProvider = new ServiceCollection()
                        .AddEntityFrameworkInMemoryDatabase()
                        .BuildServiceProvider();

                    services.AddDbContext<ApplicationDbContext>(options =>
                    {
                        options.UseInMemoryDatabase(Guid.NewGuid().ToString());
                        options.UseInternalServiceProvider(serviceProvider);
                    });

                    services.AddScoped<ApplicationDbContext>(sp =>
                        ApplicationDbContextFactory.Create());
                });
            })
            .CreateClient();

    protected HttpClient HttpClient { get; }
}
