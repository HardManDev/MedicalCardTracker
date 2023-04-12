// Copyright (c) 2023 Mikulchik Vladislav Alekseevich <hardman.dev@pm.me>.
// This software is licensed under the MIT license.
// Please see the LICENSE file for more information.

using MedicalCardTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MedicalCardTracker.Application.Server.Interfaces;

public interface IApplicationDbContext
{
    DbSet<CardRequest> CardRequests { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
