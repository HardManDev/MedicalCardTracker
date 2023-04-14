// Copyright (c) 2023 Mikulchik Vladislav Alekseevich <hardman.dev@pm.me>.
// This software is licensed under the MIT license.
// Please see the LICENSE file for more information.

using MedicalCardTracker.Database;
using MedicalCardTracker.Tests.Fixtures;
using Microsoft.EntityFrameworkCore;

namespace MedicalCardTracker.Tests.Database;

public static class ApplicationDbContextFactory
{
    public static ApplicationDbContext Create()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var dbContext = new ApplicationDbContext(options);
        dbContext.Database.EnsureCreated();

        dbContext.CardRequests.AddRange(
            FixtureCardRequests.CardRequests
                .Select(kv => kv.Value));

        dbContext.SaveChanges();

        return dbContext;
    }

    public static void Destroy(ApplicationDbContext dbContext)
    {
        dbContext.Database.EnsureDeleted();
        dbContext.Dispose();
    }
}
