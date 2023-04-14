// Copyright (c) 2023 Mikulchik Vladislav Alekseevich <hardman.dev@pm.me>.
// This software is licensed under the MIT license.
// Please see the LICENSE file for more information.

using MediatR;
using MedicalCardTracker.Application.Models.ViewModels;
using MedicalCardTracker.Application.Requests.Commands.CardRequests.CreateCardRequest;
using MedicalCardTracker.Domain.Entities;

namespace MedicalCardTracker.Application.Client.Requests.Commands.CardRequests.CreateCardRequest;

public class CreateCardRequestCommandHandler
    : BaseRequestHandler, IRequestHandler<CreateCardRequestCommand, CardRequestVm>
{
    public CreateCardRequestCommandHandler(HttpClient httpClient)
        : base(httpClient)
    {
    }

    public async Task<CardRequestVm> Handle(CreateCardRequestCommand request,
        CancellationToken cancellationToken
    ) => await SendHttpRequest(request,
        HttpMethod.Post,
        $"api/{nameof(CardRequest)}/Create",
        cancellationToken);
}
