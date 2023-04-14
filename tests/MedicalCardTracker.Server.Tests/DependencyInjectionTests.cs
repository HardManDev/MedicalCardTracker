// Copyright (c) 2023 Mikulchik Vladislav Alekseevich <hardman.dev@pm.me>.
// This software is licensed under the MIT license.
// Please see the LICENSE file for more information.

using FluentAssertions;
using MedicalCardTracker.Application.Server.Interfaces;
using MedicalCardTracker.Database;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace MedicalCardTracker.Server.Tests;

public class DependencyInjectionTests
{
    [Fact]
    public void DependencyInjection_Success()
    {
        // Arrange
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>
            {
                { "DbConnectionUrl", "Data Source=:memory:" }
            }!)
            .Build();

        var services = new ServiceCollection();

        // Act
        services.AddDatabase(configuration);

        // Assert
        var serviceProvider = services.BuildServiceProvider();
        var dbContext = serviceProvider.GetService<ApplicationDbContext>();
        var appDbContext = serviceProvider.GetService<IApplicationDbContext>();

        dbContext.Should().NotBeNull();
        dbContext.Should().BeOfType<ApplicationDbContext>();
        appDbContext.Should().NotBeNull();
        appDbContext.Should().BeOfType<ApplicationDbContext>();
    }
}
