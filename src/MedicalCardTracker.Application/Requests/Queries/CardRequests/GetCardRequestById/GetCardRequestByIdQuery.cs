// Copyright (c) 2023 Mikulchik Vladislav Alekseevich <hardman.dev@pm.me>.
// This software is licensed under the MIT license.
// Please see the LICENSE file for more information.

using MediatR;
using MedicalCardTracker.Application.Attributes;
using MedicalCardTracker.Application.Models.ViewModels;

namespace MedicalCardTracker.Application.Requests.Queries.CardRequests.GetCardRequestById;

public class GetCardRequestByIdQuery : IRequest<CardRequestVm>
{
    [QueryParameter] public Guid Id { get; set; }
}
