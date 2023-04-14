// Copyright (c) 2023 Mikulchik Vladislav Alekseevich <hardman.dev@pm.me>.
// This software is licensed under the MIT license.
// Please see the LICENSE file for more information.

using MediatR;
using MedicalCardTracker.Application.Models.ViewModels;
using MedicalCardTracker.Application.Requests.Commands.CardRequests.UpdateCardRequest;
using MedicalCardTracker.Domain.Entities;

namespace MedicalCardTracker.Application.Client.Requests.Commands.CardRequests.UpdateCardRequest;

public class UpdateCardRequestCommandHandler
    : BaseRequestHandler, IRequestHandler<UpdateCardRequestCommand, CardRequestVm>
{
    public UpdateCardRequestCommandHandler(HttpClient httpClient)
        : base(httpClient)
    {
    }

    public async Task<CardRequestVm> Handle(UpdateCardRequestCommand request,
        CancellationToken cancellationToken
    ) => await SendHttpRequest(request,
        HttpMethod.Patch,
        $"api/{nameof(CardRequest)}/Update",
        cancellationToken);
}
