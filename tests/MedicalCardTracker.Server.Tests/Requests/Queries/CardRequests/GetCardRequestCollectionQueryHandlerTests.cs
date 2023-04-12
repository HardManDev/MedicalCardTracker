// Copyright (c) 2023 Mikulchik Vladislav Alekseevich <hardman.dev@pm.me>.
// This software is licensed under the MIT license.
// Please see the LICENSE file for more information.

using System.Collections.ObjectModel;
using AutoMapper;
using FluentAssertions;
using MedicalCardTracker.Application.Models.Enums;
using MedicalCardTracker.Application.Models.ViewModels;
using MedicalCardTracker.Application.Requests.Queries.CardRequests.GetCardRequestCollection;
using MedicalCardTracker.Application.Server.Interfaces;
using MedicalCardTracker.Application.Server.Requests.Queries.CardRequests.GetCardRequestCollection;
using MedicalCardTracker.Domain.Entities;
using MedicalCardTracker.Server.Tests.Fixtures;
using Xunit;

namespace MedicalCardTracker.Server.Tests.Requests.Queries.CardRequests;

public class GetCardRequestCollectionQueryHandlerTests
    : BaseRequestHandler
{
    private readonly List<CardRequest> FixtureCardRequestList = new()
    {
        FixtureCardRequests.FixtureCardRequest,
        FixtureCardRequests.FixtureCardRequestForDelete,
        FixtureCardRequests.FixtureCardRequestForUpdate
    };

    public GetCardRequestCollectionQueryHandlerTests(IApplicationDbContext dbContext, IMapper mapper)
        : base(dbContext, mapper)
    {
        DbContext.CardRequests.RemoveRange(FixtureCardRequestList);
        DbContext.SaveChangesAsync().GetAwaiter().GetResult();
        DbContext.CardRequests.AddRange(FixtureCardRequestList);
        DbContext.SaveChangesAsync().GetAwaiter().GetResult();
    }

    [Fact]
    public async Task GetCardRequestCollectionQueryHandler_Success()
    {
        // Arrange
        var handler = new GetCardRequestCollectionQueryHandler(DbContext, Mapper);

        var command = new GetCardRequestCollectionQuery
        {
            Page = 1,
            Count = 2
        };

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeOfType<CardRequestCollectionVm>();
        result.CardRequests.Should().BeOfType<ObservableCollection<CardRequestVm>>();
        result.TotalCount.Should().Be((uint)FixtureCardRequestList.Count);
        result.CardRequests.Count.Should().Be((int)(FixtureCardRequestList.Count - command.Page * command.Count));
        result.CardRequests.Should().BeEquivalentTo(
            DbContext.CardRequests
                .OrderBy(item => item.CreatedAt)
                .Skip((int)(command.Count * command.Page))
                .Take((int)command.Count)
                .ToList());
    }

    [Fact]
    public async Task GetCardRequestCollectionQueryHandler_SuccessWithOrderBy()
    {
        // Arrange
        var handler = new GetCardRequestCollectionQueryHandler(DbContext, Mapper);

        var command = new GetCardRequestCollectionQuery
        {
            Page = 0,
            Count = 10,
            OrderBy = OrderBy.Descending
        };

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeOfType<CardRequestCollectionVm>();
        result.CardRequests.Should().BeOfType<ObservableCollection<CardRequestVm>>();
        result.TotalCount.Should().Be((uint)FixtureCardRequestList.Count);
        result.CardRequests.Count.Should().Be((int)(FixtureCardRequestList.Count - command.Page * command.Count));
        result.CardRequests.Should().BeEquivalentTo(
            DbContext.CardRequests
                .OrderByDescending(item => item.CreatedAt)
                .Skip((int)(command.Count * command.Page))
                .Take((int)command.Count)
                .ToList());
    }

    [Fact]
    public async Task GetCardRequestCollectionQueryHandler_SuccessWithSearch()
    {
        // Arrange
        var handler = new GetCardRequestCollectionQueryHandler(DbContext, Mapper);

        var command = new GetCardRequestCollectionQuery
        {
            SearchIn = nameof(CardRequest.TargetAddress),
            SearchQuery = "cab. 200"
        };

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeOfType<CardRequestCollectionVm>();
        result.CardRequests.Should().BeOfType<ObservableCollection<CardRequestVm>>();
        result.TotalCount.Should().Be((uint)FixtureCardRequestList.Count);
        result.CardRequests.Count.Should().Be(
            DbContext.CardRequests
                .Where(item => item.TargetAddress == command.SearchQuery)
                .ToList()
                .Count);
        result.CardRequests.Should().BeEquivalentTo(
            DbContext.CardRequests
                .Where(item => item.TargetAddress == command.SearchQuery)
                .OrderBy(item => item.CreatedAt)
                .Skip((int)(command.Count * command.Page))
                .Take((int)command.Count)
                .ToList());
    }
}
