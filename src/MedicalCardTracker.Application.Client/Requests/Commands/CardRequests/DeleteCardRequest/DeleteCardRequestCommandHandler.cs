// Copyright (c) 2023 Mikulchik Vladislav Alekseevich <hardman.dev@pm.me>.
// This software is licensed under the MIT license.
// Please see the LICENSE file for more information.

using MediatR;
using MedicalCardTracker.Application.Requests.Commands.CardRequests.DeleteCardRequest;
using MedicalCardTracker.Domain.Entities;

namespace MedicalCardTracker.Application.Client.Requests.Commands.CardRequests.DeleteCardRequest;

public class DeleteCardRequestCommandHandler
    : BaseRequestHandler, IRequestHandler<DeleteCardRequestCommand, Guid>
{
    public DeleteCardRequestCommandHandler(HttpClient httpClient)
        : base(httpClient)
    {
    }

    public async Task<Guid> Handle(DeleteCardRequestCommand request,
        CancellationToken cancellationToken
    ) => await SendHttpRequest(request,
        HttpMethod.Delete,
        $"api/{nameof(CardRequest)}/Delete",
        cancellationToken);
}
