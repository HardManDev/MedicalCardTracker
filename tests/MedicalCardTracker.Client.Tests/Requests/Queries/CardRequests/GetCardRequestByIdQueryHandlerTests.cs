// Copyright (c) 2023 Mikulchik Vladislav Alekseevich <hardman.dev@pm.me>.
// This software is licensed under the MIT license.
// Please see the LICENSE file for more information.

using FluentAssertions;
using MedicalCardTracker.Application.Client.Requests.Queries.CardRequests.GetCardRequestById;
using MedicalCardTracker.Application.Models.ViewModels;
using MedicalCardTracker.Application.Requests.Queries.CardRequests.GetCardRequestById;
using MedicalCardTracker.Server;
using MedicalCardTracker.Tests.Fixtures;
using MedicalCardTracker.Tests.Models.Enums;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace MedicalCardTracker.Client.Tests.Requests.Queries.CardRequests;

[Collection("CardRequestCollection")]
public class GetCardRequestByIdQueryHandlerTests
    : BaseRequestHandler
{
    public GetCardRequestByIdQueryHandlerTests(WebApplicationFactory<Program> fixture)
        : base(fixture)
    {
    }

    [Fact]
    public async Task GetCardRequestQueryHandler_Success()
    {
        // Arrange
        var query = new GetCardRequestByIdQuery
        {
            Id = FixtureCardRequests.CardRequests[FixtureDataType.Default].Id
        };
        var handler = new GetCardRequestByIdQueryHandler(HttpClient);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<CardRequestVm>();
        result.Should().BeEquivalentTo(query);
        result.Should().BeEquivalentTo(FixtureCardRequests.CardRequests[FixtureDataType.Default]);
    }

    [Fact]
    public async Task GetCardRequestQueryHandler_FailOnWrongId()
    {
        // Arrange
        var query = new GetCardRequestByIdQuery
        {
            Id = Guid.Empty
        };
        var handler = new GetCardRequestByIdQueryHandler(HttpClient);

        // Act
        var act = async () =>
            await handler.Handle(query, CancellationToken.None);

        // Assert
        await act.Should().ThrowExactlyAsync<HttpRequestException>();
    }
}
