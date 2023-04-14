// Copyright (c) 2023 Mikulchik Vladislav Alekseevich <hardman.dev@pm.me>.
// This software is licensed under the MIT license.
// Please see the LICENSE file for more information.

using FluentAssertions;
using MedicalCardTracker.Application.Requests.Commands.CardRequests.DeleteCardRequest;
using MedicalCardTracker.Application.Server.Exceptions;
using MedicalCardTracker.Application.Server.Requests.Commands.CardRequests.DeleteCardRequest;
using MedicalCardTracker.Server.Tests.Fixtures;
using MedicalCardTracker.Tests.Fixtures;
using MedicalCardTracker.Tests.Models.Enums;
using Xunit;

namespace MedicalCardTracker.Server.Tests.Requests.Commands.CardRequests;

[Collection("CardRequestCollection")]
public class DeleteCardRequestCommandHandlerTests
    : BaseRequestHandler
{
    public DeleteCardRequestCommandHandlerTests(CardRequestFixture fixture)
        : base(fixture)
    {
    }

    [Fact]
    public async Task DeleteCardRequestCommandHandler_Success()
    {
        // Arrange
        var command = new DeleteCardRequestCommand
        {
            Id = FixtureCardRequests.CardRequests[FixtureDataType.ForDelete].Id
        };
        var handler = new DeleteCardRequestCommandHandler(DbContext, Mapper);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeEmpty();
        result.Should().Be(FixtureCardRequests.CardRequests[FixtureDataType.ForDelete].Id);

        DbContext.CardRequests.Should()
            .NotContain(FixtureCardRequests.CardRequests[FixtureDataType.ForDelete]);
    }

    [Fact]
    public async Task DeleteCardRequestCommandHandler_FailOnWrongId()
    {
        // Arrange
        var command = new DeleteCardRequestCommand
        {
            Id = Guid.Empty
        };
        var handler = new DeleteCardRequestCommandHandler(DbContext, Mapper);

        // Act
        var act = async () =>
            await handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowExactlyAsync<EntityNotFoundException>();
    }
}
