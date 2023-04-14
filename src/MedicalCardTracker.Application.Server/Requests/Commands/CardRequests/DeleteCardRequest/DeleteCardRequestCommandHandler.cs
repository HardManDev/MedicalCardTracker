// Copyright (c) 2023 Mikulchik Vladislav Alekseevich <hardman.dev@pm.me>.
// This software is licensed under the MIT license.
// Please see the LICENSE file for more information.

using AutoMapper;
using MediatR;
using MedicalCardTracker.Application.Requests.Commands.CardRequests.DeleteCardRequest;
using MedicalCardTracker.Application.Server.Exceptions;
using MedicalCardTracker.Application.Server.Interfaces;
using MedicalCardTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MedicalCardTracker.Application.Server.Requests.Commands.CardRequests.DeleteCardRequest;

public class DeleteCardRequestCommandHandler
    : BaseRequestHandler, IRequestHandler<DeleteCardRequestCommand, Guid>
{
    public DeleteCardRequestCommandHandler(IApplicationDbContext dbContext, IMapper mapper)
        : base(dbContext, mapper)
    {
    }

    public async Task<Guid> Handle(DeleteCardRequestCommand request,
        CancellationToken cancellationToken)
    {
        var targetCardRequest =
            await DbContext.CardRequests
                .FirstOrDefaultAsync(item => item.Id == request.Id, cancellationToken)
            ?? throw new EntityNotFoundException(nameof(CardRequest), request.Id);

        DbContext.CardRequests.Remove(targetCardRequest);
        await DbContext.SaveChangesAsync(cancellationToken);

        return targetCardRequest.Id;
    }
}
