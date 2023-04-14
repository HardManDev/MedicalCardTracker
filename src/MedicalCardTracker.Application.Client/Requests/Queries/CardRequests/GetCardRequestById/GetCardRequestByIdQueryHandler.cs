// Copyright (c) 2023 Mikulchik Vladislav Alekseevich <hardman.dev@pm.me>.
// This software is licensed under the MIT license.
// Please see the LICENSE file for more information.

using MediatR;
using MedicalCardTracker.Application.Models.ViewModels;
using MedicalCardTracker.Application.Requests.Queries.CardRequests.GetCardRequestById;
using MedicalCardTracker.Domain.Entities;

namespace MedicalCardTracker.Application.Client.Requests.Queries.CardRequests.GetCardRequestById;

public class GetCardRequestByIdQueryHandler
    : BaseRequestHandler, IRequestHandler<GetCardRequestByIdQuery, CardRequestVm>
{
    public GetCardRequestByIdQueryHandler(HttpClient httpClient)
        : base(httpClient)
    {
    }

    public async Task<CardRequestVm> Handle(GetCardRequestByIdQuery request,
        CancellationToken cancellationToken
    ) => await SendHttpRequest(request,
        HttpMethod.Get,
        $"api/{nameof(CardRequest)}/Get",
        cancellationToken);
}
