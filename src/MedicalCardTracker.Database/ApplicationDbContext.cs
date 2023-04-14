// Copyright (c) 2023 Mikulchik Vladislav Alekseevich <hardman.dev@pm.me>.
// This software is licensed under the MIT license.
// Please see the LICENSE file for more information.

using MedicalCardTracker.Application.Server.Interfaces;
using MedicalCardTracker.Database.EntityTypeConfigurations;
using MedicalCardTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MedicalCardTracker.Database;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<CardRequest> CardRequests { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new CardRequestConfiguration());
        base.OnModelCreating(modelBuilder);
    }
}
