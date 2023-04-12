﻿// Copyright (c) 2023 Mikulchik Vladislav Alekseevich <hardman.dev@pm.me>.
// This software is licensed under the MIT license.
// Please see the LICENSE file for more information.

using AutoMapper;
using FluentAssertions;
using MedicalCardTracker.Application.Models.ViewModels;
using MedicalCardTracker.Application.Requests.Commands.CardRequests.UpdateCardRequest;
using MedicalCardTracker.Application.Server.Exceptions;
using MedicalCardTracker.Application.Server.Interfaces;
using MedicalCardTracker.Application.Server.Requests.Commands.CardRequests.UpdateCardRequest;
using MedicalCardTracker.Domain.Enums;
using MedicalCardTracker.Server.Tests.Fixtures;
using Xunit;

namespace MedicalCardTracker.Server.Tests.Requests.Commands.CardRequests;

public class UpdateCardRequestCommandHandlerTests
    : BaseRequestHandler
{
    public UpdateCardRequestCommandHandlerTests(IApplicationDbContext dbContext, IMapper mapper)
        : base(dbContext, mapper)
    {
    }

    [Fact]
    public async Task UpdateCardRequestCommandHandler_Success()
    {
        // Arrange
        var handler = new UpdateCardRequestCommandHandler(DbContext, Mapper);

        var command = new UpdateCardRequestCommand
        {
            Id = FixtureCardRequests.FixtureCardRequestForUpdate.Id,
            CustomerName = "Elesin P. H.",
            TargetAddress = "cab. 505",
            PatientFullName = "Zhivopiscev Diomid Mitrofanievich",
            PatientBirthDate = new DateOnly(1970, 8, 14),
            Description = "this is test card request",
            Status = CardRequestStatus.Created,
            Priority = CardRequestPriority.Urgently
        };

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeOfType<CardRequestVm>();
        DbContext.CardRequests.Should().ContainSingle(x =>
            x.Id != Guid.Empty &&
            x.CustomerName == command.CustomerName &&
            x.TargetAddress == command.TargetAddress &&
            x.PatientFullName == command.PatientFullName &&
            x.PatientBirthDate == command.PatientBirthDate &&
            x.Description == command.Description &&
            x.Priority == command.Priority &&
            x.CreatedAt.Date == DateTime.Now.Date &&
            x.UpdatedAt != null &&
            x.UpdatedAt.Value.Date == DateTime.Now.Date);
    }

    [Fact]
    public async Task UpdateCardRequestCommandHandler_FailOnWrongId()
    {
        // Arrange
        var handler = new UpdateCardRequestCommandHandler(DbContext, Mapper);

        // Act & Assert
        await Assert.ThrowsAsync<EntityNotFoundException>(async () =>
            await handler.Handle(
                new UpdateCardRequestCommand
                {
                    Id = Guid.Empty,
                    Priority = CardRequestPriority.UnUrgently
                }, CancellationToken.None));
    }
}
