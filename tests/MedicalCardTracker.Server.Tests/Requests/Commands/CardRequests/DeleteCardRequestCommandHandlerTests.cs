// Copyright (c) 2023 Mikulchik Vladislav Alekseevich <hardman.dev@pm.me>.
// This software is licensed under the MIT license.
// Please see the LICENSE file for more information.

using AutoMapper;
using FluentAssertions;
using MedicalCardTracker.Application.Requests.Commands.CardRequests.DeleteCardRequest;
using MedicalCardTracker.Application.Server.Exceptions;
using MedicalCardTracker.Application.Server.Interfaces;
using MedicalCardTracker.Application.Server.Requests.Commands.CardRequests.DeleteCardRequest;
using MedicalCardTracker.Server.Tests.Fixtures;
using Xunit;

namespace MedicalCardTracker.Server.Tests.Requests.Commands.CardRequests;

public class DeleteCardRequestCommandHandlerTests
    : BaseRequestHandler
{
    public DeleteCardRequestCommandHandlerTests(IApplicationDbContext dbContext, IMapper mapper)
        : base(dbContext, mapper)
    {
    }

    [Fact]
    public async Task DeleteCardRequestCommandHandler_Success()
    {
        // Arrange
        var handler = new DeleteCardRequestCommandHandler(DbContext, Mapper);
        var cardRequestToDelete = FixtureCardRequests.FixtureCardRequestForDelete;

        // Act
        var result = await handler.Handle(
            new DeleteCardRequestCommand
            {
                Id = cardRequestToDelete.Id
            },
            CancellationToken.None);

        // Assert
        result.Should().Be(FixtureCardRequests.FixtureCardRequestForDelete.Id);
        result.Should().NotBeEmpty();
        DbContext.CardRequests.Should().NotContain(cardRequestToDelete);
    }

    [Fact]
    public async Task DeleteCardRequestCommandHandler_FailOnWrongId()
    {
        // Arrange
        var handler = new DeleteCardRequestCommandHandler(DbContext, Mapper);

        // Act & Assert
        await Assert.ThrowsAsync<EntityNotFoundException>(async () =>
            await handler.Handle(new DeleteCardRequestCommand { Id = Guid.Empty },
                CancellationToken.None));
    }
}
