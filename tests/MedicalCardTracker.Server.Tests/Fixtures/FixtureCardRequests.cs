// Copyright (c) 2023 Mikulchik Vladislav Alekseevich <hardman.dev@pm.me>.
// This software is licensed under the MIT license.
// Please see the LICENSE file for more information.

using MedicalCardTracker.Domain.Entities;
using MedicalCardTracker.Domain.Enums;

namespace MedicalCardTracker.Server.Tests.Fixtures;

public static class FixtureCardRequests
{
    public static readonly CardRequest FixtureCardRequest = new()
    {
        Id = Guid.NewGuid(),
        CustomerName = "Kondrashkin I. L.",
        TargetAddress = "cab. 100",
        PatientFullName = "Poluvetrova Olimpiada Glebovna",
        PatientBirthDate = new DateOnly(1976, 4, 22),
        Description = "this is fixture card request",
        Status = CardRequestStatus.Created,
        Priority = CardRequestPriority.Urgently,
        CreatedAt = DateTime.Now,
        UpdatedAt = null
    };

    public static readonly CardRequest FixtureCardRequestForDelete = new()
    {
        Id = Guid.NewGuid(),
        CustomerName = "Pushkarev N. K.",
        TargetAddress = "cab. 200",
        PatientFullName = "Levencov Antrop Alfredovich",
        PatientBirthDate = new DateOnly(1956, 11, 5),
        Description = "this is fixture card request for delete",
        Status = CardRequestStatus.Completed,
        Priority = CardRequestPriority.UnUrgently,
        CreatedAt = DateTime.Now.AddHours(12),
        UpdatedAt = DateTime.Now
    };

    public static readonly CardRequest FixtureCardRequestForUpdate = new()
    {
        Id = Guid.NewGuid(),
        CustomerName = "Luchinin A. I.",
        TargetAddress = "cab. 200",
        PatientFullName = "Yevgeniya Vinogradova",
        PatientBirthDate = new DateOnly(2005, 1, 14),
        Description = "Volkonskaya Yuliya Porfirovna",
        Status = CardRequestStatus.Created,
        Priority = CardRequestPriority.Urgently,
        CreatedAt = DateTime.Now,
        UpdatedAt = null
    };
}
