// Copyright (c) 2023 Mikulchik Vladislav Alekseevich <hardman.dev@pm.me>.
// This software is licensed under the MIT license.
// Please see the LICENSE file for more information.

using AutoMapper;
using MediatR;
using MedicalCardTracker.Application.Models.ViewModels;
using MedicalCardTracker.Application.Requests.Commands.CardRequests.CreateCardRequest;
using MedicalCardTracker.Application.Server.Interfaces;
using MedicalCardTracker.Domain.Entities;
using MedicalCardTracker.Domain.Enums;

namespace MedicalCardTracker.Application.Server.Requests.Commands.CardRequests.CreateCardRequest;

public class CreateCardRequestCommandHandler
    : BaseRequestHandler, IRequestHandler<CreateCardRequestCommand, CardRequestVm>
{
    public CreateCardRequestCommandHandler(IApplicationDbContext dbContext, IMapper mapper)
        : base(dbContext, mapper)
    {
    }

    public async Task<CardRequestVm> Handle(CreateCardRequestCommand request,
        CancellationToken cancellationToken)
    {
        CardRequest newCardRequest = new()
        {
            Id = Guid.NewGuid(),
            CustomerName = request.CustomerName,
            TargetAddress = request.TargetAddress,
            PatientFullName = request.PatientFullName,
            PatientBirthDate = request.PatientBirthDate,
            Description = request.Description,
            Status = CardRequestStatus.Created,
            Priority = request.Priority,
            CreatedAt = DateTime.Now,
            UpdatedAt = null
        };

        await DbContext.CardRequests.AddAsync(newCardRequest, cancellationToken);
        await DbContext.SaveChangesAsync(cancellationToken);

        return Mapper.Map<CardRequestVm>(newCardRequest);
    }
}
