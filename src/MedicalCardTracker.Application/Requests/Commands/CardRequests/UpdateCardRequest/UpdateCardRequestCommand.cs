// Copyright (c) 2023 Mikulchik Vladislav Alekseevich <hardman.dev@pm.me>.
// This software is licensed under the MIT license.
// Please see the LICENSE file for more information.

using MediatR;
using MedicalCardTracker.Application.Attributes;
using MedicalCardTracker.Application.Models.ViewModels;
using MedicalCardTracker.Domain.Enums;

namespace MedicalCardTracker.Application.Requests.Commands.CardRequests.UpdateCardRequest;

public class UpdateCardRequestCommand : IRequest<CardRequestVm>
{
    [QueryParameter] public Guid Id { get; set; }

    public string? CustomerName { get; set; }
    public string? TargetAddress { get; set; }

    public string? PatientFullName { get; set; }
    public DateOnly? PatientBirthDate { get; set; }

    public string? Description { get; set; }

    public CardRequestStatus? Status { get; set; }
    public CardRequestPriority? Priority { get; set; }
}
