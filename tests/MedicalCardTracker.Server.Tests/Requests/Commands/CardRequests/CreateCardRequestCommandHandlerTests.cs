// Copyright (c) 2023 Mikulchik Vladislav Alekseevich <hardman.dev@pm.me>.
// This software is licensed under the MIT license.
// Please see the LICENSE file for more information.

using FluentAssertions;
using MedicalCardTracker.Application.Models.ViewModels;
using MedicalCardTracker.Application.Requests.Commands.CardRequests.CreateCardRequest;
using MedicalCardTracker.Application.Server.Requests.Commands.CardRequests.CreateCardRequest;
using MedicalCardTracker.Domain.Enums;
using MedicalCardTracker.Server.Tests.Fixtures;
using Xunit;

namespace MedicalCardTracker.Server.Tests.Requests.Commands.CardRequests;

[Collection("CardRequestCollection")]
public class CreateCardRequestCommandHandlerTests
    : BaseRequestHandler
{
    public CreateCardRequestCommandHandlerTests(CardRequestFixture fixture)
        : base(fixture)
    {
    }

    [Fact]
    public async Task CreateCardRequestCommandHandler_Success()
    {
        // Arrange
        var command = new CreateCardRequestCommand
        {
            CustomerName = "Shavirin S. V.",
            TargetAddress = "cab. 404",
            PatientFullName = "Zhivopiscev Diomid Mitrofanievich",
            PatientBirthDate = new DateOnly(2000, 12, 30),
            Description = "this is test card request",
            Priority = CardRequestPriority.Urgently
        };
        var handler = new CreateCardRequestCommandHandler(DbContext, Mapper);

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
            x.UpdatedAt == null);
    }
}
