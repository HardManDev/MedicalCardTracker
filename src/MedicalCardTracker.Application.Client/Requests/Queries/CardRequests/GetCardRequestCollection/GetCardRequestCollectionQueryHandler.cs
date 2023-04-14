// Copyright (c) 2023 Mikulchik Vladislav Alekseevich <hardman.dev@pm.me>.
// This software is licensed under the MIT license.
// Please see the LICENSE file for more information.

using MediatR;
using MedicalCardTracker.Application.Models.ViewModels;
using MedicalCardTracker.Application.Requests.Queries.CardRequests.GetCardRequestCollection;
using MedicalCardTracker.Domain.Entities;

namespace MedicalCardTracker.Application.Client.Requests.Queries.CardRequests.GetCardRequestCollection;

public class GetCardRequestCollectionQueryHandler
    : BaseRequestHandler, IRequestHandler<GetCardRequestCollectionQuery, CardRequestCollectionVm>
{
    public GetCardRequestCollectionQueryHandler(HttpClient httpClient)
        : base(httpClient)
    {
    }

    public async Task<CardRequestCollectionVm> Handle(GetCardRequestCollectionQuery request,
        CancellationToken cancellationToken
    ) => await SendHttpRequest(request,
        HttpMethod.Get,
        $"api/{nameof(CardRequest)}/GetCollection",
        cancellationToken);
}
