// Copyright (c) 2023 Mikulchik Vladislav Alekseevich <hardman.dev@pm.me>.
// This software is licensed under the MIT license.
// Please see the LICENSE file for more information.

using AutoMapper;
using MediatR;
using MedicalCardTracker.Application.Models.ViewModels;
using MedicalCardTracker.Application.Requests.Commands.CardRequests.UpdateCardRequest;
using MedicalCardTracker.Application.Server.Exceptions;
using MedicalCardTracker.Application.Server.Interfaces;
using MedicalCardTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MedicalCardTracker.Application.Server.Requests.Commands.CardRequests.UpdateCardRequest;

public class UpdateCardRequestCommandHandler
    : BaseRequestHandler, IRequestHandler<UpdateCardRequestCommand, CardRequestVm>
{
    public UpdateCardRequestCommandHandler(IApplicationDbContext dbContext, IMapper mapper)
        : base(dbContext, mapper)
    {
    }

    public async Task<CardRequestVm> Handle(UpdateCardRequestCommand request,
        CancellationToken cancellationToken)
    {
        var targetCardRequest =
            await DbContext.CardRequests
                .FirstOrDefaultAsync(item => item.Id == request.Id, cancellationToken)
            ?? throw new EntityNotFoundException(nameof(CardRequest), request.Id);

        targetCardRequest.CustomerName = request.CustomerName ?? targetCardRequest.CustomerName;
        targetCardRequest.TargetAddress = request.TargetAddress ?? targetCardRequest.TargetAddress;
        targetCardRequest.PatientFullName = request.PatientFullName ?? targetCardRequest.PatientFullName;
        targetCardRequest.PatientBirthDate = request.PatientBirthDate ?? targetCardRequest.PatientBirthDate;
        targetCardRequest.Description = request.Description ?? targetCardRequest.Description;
        targetCardRequest.Status = request.Status ?? targetCardRequest.Status;
        targetCardRequest.Priority = request.Priority ?? targetCardRequest.Priority;
        targetCardRequest.UpdatedAt = DateTime.Now;

        await DbContext.SaveChangesAsync(cancellationToken);

        return Mapper.Map<CardRequestVm>(targetCardRequest);
    }
}
