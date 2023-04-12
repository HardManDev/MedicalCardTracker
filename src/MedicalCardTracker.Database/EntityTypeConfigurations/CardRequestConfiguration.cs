// Copyright (c) 2023 Mikulchik Vladislav Alekseevich <hardman.dev@pm.me>.
// This software is licensed under the MIT license.
// Please see the LICENSE file for more information.

using MedicalCardTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedicalCardTracker.Database.EntityTypeConfigurations;

public class CardRequestConfiguration : IEntityTypeConfiguration<CardRequest>
{
    public void Configure(EntityTypeBuilder<CardRequest> builder)
    {
        builder.HasKey(item => item.Id);
        builder.HasIndex(item => item.TargetAddress);
        builder.HasIndex(item => item.CreatedAt);
    }
}
