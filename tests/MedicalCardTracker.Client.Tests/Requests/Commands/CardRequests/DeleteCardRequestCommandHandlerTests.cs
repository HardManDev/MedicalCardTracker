// Copyright (c) 2023 Mikulchik Vladislav Alekseevich <hardman.dev@pm.me>.
// This software is licensed under the MIT license.
// Please see the LICENSE file for more information.

using FluentAssertions;
using MedicalCardTracker.Application.Client.Requests.Commands.CardRequests.DeleteCardRequest;
using MedicalCardTracker.Application.Requests.Commands.CardRequests.DeleteCardRequest;
using MedicalCardTracker.Server;
using MedicalCardTracker.Tests.Fixtures;
using MedicalCardTracker.Tests.Models.Enums;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace MedicalCardTracker.Client.Tests.Requests.Commands.CardRequests;

[Collection("CardRequestCollection")]
public class DeleteCardRequestCommandHandlerTests
    : BaseRequestHandler
{
    public DeleteCardRequestCommandHandlerTests(WebApplicationFactory<Program> fixture)
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
        var handler = new DeleteCardRequestCommandHandler(HttpClient);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeEmpty();
        result.Should().Be(command.Id);
    }

    [Fact]
    public async Task DeleteCardRequestCommandHandler_FailOnWrongId()
    {
        // Arrange
        var command = new DeleteCardRequestCommand
        {
            Id = Guid.Empty
        };
        var handler = new DeleteCardRequestCommandHandler(HttpClient);

        // Act
        var act = async () =>
            await handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowExactlyAsync<HttpRequestException>();
    }
}
