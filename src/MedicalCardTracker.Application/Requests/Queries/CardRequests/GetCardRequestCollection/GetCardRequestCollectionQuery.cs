﻿// Copyright (c) 2023 Mikulchik Vladislav Alekseevich <hardman.dev@pm.me>.
// This software is licensed under the MIT license.
// Please see the LICENSE file for more information.

using MediatR;
using MedicalCardTracker.Application.Attributes;
using MedicalCardTracker.Application.Models.Enums;
using MedicalCardTracker.Application.Models.ViewModels;

namespace MedicalCardTracker.Application.Requests.Queries.CardRequests.GetCardRequestCollection;

public class GetCardRequestCollectionQuery : IRequest<CardRequestCollectionVm>
{
    [QueryParameter] public uint Page { get; set; } = 0;
    [QueryParameter] public uint Count { get; set; } = 100;

    [QueryParameter] public string? SearchIn { get; set; }
    [QueryParameter] public string? SearchQuery { get; set; }
    [QueryParameter] public OrderBy OrderBy { get; set; } = OrderBy.Ascending;
}
