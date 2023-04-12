// Copyright (c) 2023 Mikulchik Vladislav Alekseevich <hardman.dev@pm.me>.
// This software is licensed under the MIT license.
// Please see the LICENSE file for more information.

using MediatR;
using MedicalCardTracker.Application.Attributes;

namespace MedicalCardTracker.Application.Requests.Commands.CardRequests.DeleteCardRequest;

public class DeleteCardRequestCommand : IRequest
{
    [QueryParameter] public Guid Id { get; set; }
}
