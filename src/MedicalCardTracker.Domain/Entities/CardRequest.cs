// Copyright (c) 2023 Mikulchik Vladislav Alekseevich <hardman.dev@pm.me>.
// This software is licensed under the MIT license.
// Please see the LICENSE file for more information.

using MedicalCardTracker.Domain.Enums;

namespace MedicalCardTracker.Domain.Entities;

public class CardRequest
{
    public Guid Id { get; set; }

    public string CustomerName { get; set; } = null!;
    public string TargetAddress { get; set; } = null!;

    public string PatientFullName { get; set; } = null!;
    public DateOnly? PatientBirthDate { get; set; }

    public string? Description { get; set; }

    public CardRequestStatus Status { get; set; }
    public CardRequestPriority Priority { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
