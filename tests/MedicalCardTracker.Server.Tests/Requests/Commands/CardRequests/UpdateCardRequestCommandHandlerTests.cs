// Copyright (c) 2023 Mikulchik Vladislav Alekseevich <hardman.dev@pm.me>.
// This software is licensed under the MIT license.
// Please see the LICENSE file for more information.

using FluentAssertions;
using MedicalCardTracker.Application.Models.ViewModels;
using MedicalCardTracker.Application.Requests.Commands.CardRequests.UpdateCardRequest;
using MedicalCardTracker.Application.Server.Exceptions;
using MedicalCardTracker.Application.Server.Requests.Commands.CardRequests.UpdateCardRequest;
using MedicalCardTracker.Domain.Enums;
using MedicalCardTracker.Server.Tests.Fixtures;
using MedicalCardTracker.Tests.Fixtures;
using MedicalCardTracker.Tests.Models.Enums;
using Xunit;

namespace MedicalCardTracker.Server.Tests.Requests.Commands.CardRequests;

[Collection("CardRequestCollection")]
public class UpdateCardRequestCommandHandlerTests
    : BaseRequestHandler
{
    public UpdateCardRequestCommandHandlerTests(CardRequestFixture fixture)
        : base(fixture)
    {
    }

    [Fact]
    public async Task UpdateCardRequestCommandHandler_Success()
    {
        // Arrange
        var command = new UpdateCardRequestCommand
        {
            Id = FixtureCardRequests.CardRequests[FixtureDataType.ForUpdate].Id,
            CustomerName = "Elesin P. H.",
            TargetAddress = "cab. 505",
            PatientFullName = "Zhivopiscev Diomid Mitrofanievich",
            PatientBirthDate = new DateOnly(1970, 8, 14),
            Description = "this is test card request",
            Status = CardRequestStatus.Created,
            Priority = CardRequestPriority.Urgently
        };
        var handler = new UpdateCardRequestCommandHandler(DbContext, Mapper);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<CardRequestVm>();
        result.Should().BeEquivalentTo(command);

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
        var command = new UpdateCardRequestCommand
        {
            Id = Guid.Empty
        };
        var handler = new UpdateCardRequestCommandHandler(DbContext, Mapper);

        // Act
        var act = async () =>
            await handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowExactlyAsync<EntityNotFoundException>();
    }
}
