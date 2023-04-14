// Copyright (c) 2023 Mikulchik Vladislav Alekseevich <hardman.dev@pm.me>.
// This software is licensed under the MIT license.
// Please see the LICENSE file for more information.

using FluentAssertions;
using MedicalCardTracker.Application.Client.Requests.Commands.CardRequests.UpdateCardRequest;
using MedicalCardTracker.Application.Models.ViewModels;
using MedicalCardTracker.Application.Requests.Commands.CardRequests.UpdateCardRequest;
using MedicalCardTracker.Domain.Enums;
using MedicalCardTracker.Server;
using MedicalCardTracker.Tests.Fixtures;
using MedicalCardTracker.Tests.Models.Enums;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace MedicalCardTracker.Client.Tests.Requests.Commands.CardRequests;

[Collection("CardRequestCollection")]
public class UpdateCardRequestCommandHandlerTests
    : BaseRequestHandler
{
    public UpdateCardRequestCommandHandlerTests(WebApplicationFactory<Program> fixture)
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
            CustomerName = "Mikulchik V. A.",
            TargetAddress = "cab. 404",
            PatientFullName = "Mikulchik Vladislav Alekseevich",
            PatientBirthDate = DateOnly.MinValue,
            Description = "This is test card request",
            Status = CardRequestStatus.Completed,
            Priority = CardRequestPriority.Urgently
        };
        var handler = new UpdateCardRequestCommandHandler(HttpClient);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeOfType<CardRequestVm>();
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(command);
    }

    [Fact]
    public async Task UpdateCardRequestCommandHandler_FailOnWrongId()
    {
        // Arrange
        var command = new UpdateCardRequestCommand
        {
            Id = Guid.Empty
        };
        var handler = new UpdateCardRequestCommandHandler(HttpClient);

        // Act
        var act = async () =>
            await handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowExactlyAsync<HttpRequestException>();
    }
}
