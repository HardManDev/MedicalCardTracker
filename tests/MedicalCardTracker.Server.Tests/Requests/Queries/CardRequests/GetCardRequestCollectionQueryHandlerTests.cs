// Copyright (c) 2023 Mikulchik Vladislav Alekseevich <hardman.dev@pm.me>.
// This software is licensed under the MIT license.
// Please see the LICENSE file for more information.

using System.Collections.ObjectModel;
using FluentAssertions;
using MedicalCardTracker.Application.Models.Enums;
using MedicalCardTracker.Application.Models.ViewModels;
using MedicalCardTracker.Application.Requests.Queries.CardRequests.GetCardRequestCollection;
using MedicalCardTracker.Application.Server.Requests.Queries.CardRequests.GetCardRequestCollection;
using MedicalCardTracker.Domain.Entities;
using MedicalCardTracker.Server.Tests.Fixtures;
using MedicalCardTracker.Tests.Fixtures;
using Xunit;

namespace MedicalCardTracker.Server.Tests.Requests.Queries.CardRequests;

[Collection("CardRequestCollection")]
public class GetCardRequestCollectionQueryHandlerTests
    : BaseRequestHandler
{
    public GetCardRequestCollectionQueryHandlerTests(CardRequestFixture fixture)
        : base(fixture)
    {
    }

    [Fact]
    public async Task GetCardRequestCollectionQueryHandler_Success()
    {
        // Arrange
        var query = new GetCardRequestCollectionQuery
        {
            Page = 1,
            Count = 2
        };
        var handler = new GetCardRequestCollectionQueryHandler(DbContext, Mapper);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<CardRequestCollectionVm>();

        result.CardRequests.Should().BeOfType<ObservableCollection<CardRequestVm>>();
        result.TotalCount.Should().Be((uint)FixtureCardRequests.CardRequests.Count);
        result.CardRequests.Count.Should().Be((int)(FixtureCardRequests.CardRequests.Count - query.Page * query.Count));

        result.CardRequests.Should().BeEquivalentTo(
            DbContext.CardRequests
                .OrderBy(item => item.CreatedAt)
                .Skip((int)(query.Count * query.Page))
                .Take((int)query.Count)
                .ToList());
    }

    [Fact]
    public async Task GetCardRequestCollectionQueryHandler_SuccessWithOrderBy()
    {
        // Arrange
        var query = new GetCardRequestCollectionQuery
        {
            OrderBy = OrderBy.Descending
        };
        var handler = new GetCardRequestCollectionQueryHandler(DbContext, Mapper);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<CardRequestCollectionVm>();

        result.CardRequests.Should().BeOfType<ObservableCollection<CardRequestVm>>();
        result.TotalCount.Should().Be((uint)FixtureCardRequests.CardRequests.Count);
        result.CardRequests.Count.Should().Be((int)(FixtureCardRequests.CardRequests.Count - query.Page * query.Count));

        result.CardRequests.Should().BeEquivalentTo(
            DbContext.CardRequests
                .OrderByDescending(item => item.CreatedAt)
                .Skip((int)(query.Count * query.Page))
                .Take((int)query.Count)
                .ToList());
    }

    [Fact]
    public async Task GetCardRequestCollectionQueryHandler_SuccessWithSearch()
    {
        // Arrange
        var query = new GetCardRequestCollectionQuery
        {
            SearchIn = nameof(CardRequest.TargetAddress),
            SearchQuery = "cab. 200"
        };
        var handler = new GetCardRequestCollectionQueryHandler(DbContext, Mapper);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<CardRequestCollectionVm>();

        result.CardRequests.Should().BeOfType<ObservableCollection<CardRequestVm>>();
        result.TotalCount.Should().Be((uint)FixtureCardRequests.CardRequests.Count);

        result.CardRequests.Count.Should().Be(
            DbContext.CardRequests
                .Where(item => item.TargetAddress == query.SearchQuery)
                .ToList()
                .Count);
        result.CardRequests.Should().BeEquivalentTo(
            DbContext.CardRequests
                .Where(item => item.TargetAddress == query.SearchQuery)
                .OrderBy(item => item.CreatedAt)
                .Skip((int)(query.Count * query.Page))
                .Take((int)query.Count)
                .ToList());
    }
}
