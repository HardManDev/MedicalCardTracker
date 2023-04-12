// Copyright (c) 2023 Mikulchik Vladislav Alekseevich <hardman.dev@pm.me>.
// This software is licensed under the MIT license.
// Please see the LICENSE file for more information.

using System.Text.Json.Serialization;
using MediatR;
using MedicalCardTracker.Application.Convertors;
using MedicalCardTracker.Application.Models.ViewModels;
using MedicalCardTracker.Domain.Enums;

namespace MedicalCardTracker.Application.Requests.Commands.CardRequests.CreateCardRequest;

public class CreateCardRequestCommand : IRequest<CardRequestVm>
{
    public string CustomerName { get; set; } = null!;
    public string TargetAddress { get; set; } = null!;

    public string PatientFullName { get; set; } = null!;

    [JsonConverter(typeof(DateOnlyConverter))]
    public DateOnly? PatientBirthDate { get; set; }

    public string? Description { get; set; }

    public CardRequestPriority Priority { get; set; }
}
