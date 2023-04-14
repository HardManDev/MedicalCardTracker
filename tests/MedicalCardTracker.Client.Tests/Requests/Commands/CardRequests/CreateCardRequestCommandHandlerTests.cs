// Copyright (c) 2023 Mikulchik Vladislav Alekseevich <hardman.dev@pm.me>.
// This software is licensed under the MIT license.
// Please see the LICENSE file for more information.

using FluentAssertions;
using MedicalCardTracker.Application.Client.Requests.Commands.CardRequests.CreateCardRequest;
using MedicalCardTracker.Application.Models.ViewModels;
using MedicalCardTracker.Application.Requests.Commands.CardRequests.CreateCardRequest;
using MedicalCardTracker.Domain.Enums;
using MedicalCardTracker.Server;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace MedicalCardTracker.Client.Tests.Requests.Commands.CardRequests;

[Collection("CardRequestCollection")]
public class CreateCardRequestCommandHandlerTests
    : BaseRequestHandler
{
    public CreateCardRequestCommandHandlerTests(WebApplicationFactory<Program> fixture)
        : base(fixture)
    {
    }

    [Fact]
    public async Task CreateCardRequestCommandHandler_Success()
    {
        // Arrange
        var command = new CreateCardRequestCommand
        {
            CustomerName = "Mikulchik V. A.",
            TargetAddress = "cab. 404",
            PatientFullName = "Mikulchik Vladislav Alekseevich",
            PatientBirthDate = DateOnly.MinValue,
            Description = "This is test card request",
            Priority = CardRequestPriority.Urgently
        };
        var handler = new CreateCardRequestCommandHandler(HttpClient);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeOfType<CardRequestVm>();
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(command);
    }
}
