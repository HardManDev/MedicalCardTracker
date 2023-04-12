// Copyright (c) 2023 Mikulchik Vladislav Alekseevich <hardman.dev@pm.me>.
// This software is licensed under the MIT license.
// Please see the LICENSE file for more information.

using System.Text.Json.Serialization;
using MedicalCardTracker.Application.Convertors;
using MedicalCardTracker.Domain.Enums;

namespace MedicalCardTracker.Server.Models.Dto;

public class UpdateCardRequestCommandDto
{
    public string? CustomerName { get; set; }
    public string? TargetAddress { get; set; }

    public string? PatientFullName { get; set; }

    [JsonConverter(typeof(DateOnlyConverter))]
    public DateOnly? PatientBirthDate { get; set; }

    public string? Description { get; set; }

    public CardRequestStatus? Status { get; set; }
    public CardRequestPriority? Priority { get; set; }
}
