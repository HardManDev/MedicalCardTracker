// Copyright (c) 2023 Mikulchik Vladislav Alekseevich <hardman.dev@pm.me>.
// This software is licensed under the MIT license.
// Please see the LICENSE file for more information.

using AutoMapper;
using MediatR;
using MedicalCardTracker.Application.Models.ViewModels;
using MedicalCardTracker.Application.Requests.Queries.CardRequests.GetCardRequestById;
using MedicalCardTracker.Application.Server.Exceptions;
using MedicalCardTracker.Application.Server.Interfaces;
using MedicalCardTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MedicalCardTracker.Application.Server.Requests.Queries.CardRequests.GetCardRequestById;

public class GetCardRequestByIdQueryHandler
    : BaseRequestHandler, IRequestHandler<GetCardRequestByIdQuery, CardRequestVm>
{
    public GetCardRequestByIdQueryHandler(IApplicationDbContext dbContext, IMapper mapper)
        : base(dbContext, mapper)
    {
    }

    public async Task<CardRequestVm> Handle(GetCardRequestByIdQuery request,
        CancellationToken cancellationToken)
    {
        var targetCardRequest =
            await DbContext.CardRequests
                .FirstOrDefaultAsync(item => item.Id == request.Id, cancellationToken)
            ?? throw new EntityNotFoundException(nameof(CardRequest), request.Id);

        return Mapper.Map<CardRequestVm>(targetCardRequest);
    }
}
