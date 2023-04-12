// Copyright (c) 2023 Mikulchik Vladislav Alekseevich <hardman.dev@pm.me>.
// This software is licensed under the MIT license.
// Please see the LICENSE file for more information.

using AutoMapper;
using FluentAssertions;
using MedicalCardTracker.Application.Models.ViewModels;
using MedicalCardTracker.Application.Requests.Queries.CardRequests.GetCardRequestById;
using MedicalCardTracker.Application.Server.Exceptions;
using MedicalCardTracker.Application.Server.Interfaces;
using MedicalCardTracker.Application.Server.Requests.Queries.CardRequests.GetCardRequestById;
using MedicalCardTracker.Server.Tests.Fixtures;
using Xunit;

namespace MedicalCardTracker.Server.Tests.Requests.Queries.CardRequests;

public class GetCardRequestByIdQueryHandlerTests
    : BaseRequestHandler
{
    public GetCardRequestByIdQueryHandlerTests(IApplicationDbContext dbContext, IMapper mapper)
        : base(dbContext, mapper)
    {
    }

    [Fact]
    public async Task GetCardRequestQueryHandler_Success()
    {
        // Arrange
        var handler = new GetCardRequestByIdQueryHandler(DbContext, Mapper);

        // Act
        var result = await handler.Handle(
            new GetCardRequestByIdQuery
            {
                Id = FixtureCardRequests.FixtureCardRequest.Id
            },
            CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(
            Mapper.Map<CardRequestVm>(DbContext.CardRequests
                .FirstOrDefault(item =>
                    item.Id == FixtureCardRequests.FixtureCardRequest.Id)));
    }

    [Fact]
    public async Task GetCardRequestQueryHandler_FailOnWrongId()
    {
        // Arrange
        var handler = new GetCardRequestByIdQueryHandler(DbContext, Mapper);

        // Act && Assert
        await Assert.ThrowsAsync<EntityNotFoundException>(async () =>
            await handler.Handle(
                new GetCardRequestByIdQuery
                {
                    Id = Guid.Empty
                }, CancellationToken.None));
    }
}
