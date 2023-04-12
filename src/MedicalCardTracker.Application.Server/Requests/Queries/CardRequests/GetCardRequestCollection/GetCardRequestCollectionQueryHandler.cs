// Copyright (c) 2023 Mikulchik Vladislav Alekseevich <hardman.dev@pm.me>.
// This software is licensed under the MIT license.
// Please see the LICENSE file for more information.

using System.Collections.ObjectModel;
using AutoMapper;
using MediatR;
using MedicalCardTracker.Application.Models;
using MedicalCardTracker.Application.Models.Enums;
using MedicalCardTracker.Application.Models.ViewModels;
using MedicalCardTracker.Application.Requests.Queries.CardRequests.GetCardRequestCollection;
using MedicalCardTracker.Application.Server.Interfaces;
using MedicalCardTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MedicalCardTracker.Application.Server.Requests.Queries.CardRequests.GetCardRequestCollection;

public class GetCardRequestCollectionQueryHandler
    : BaseRequestHandler, IRequestHandler<GetCardRequestCollectionQuery, CardRequestCollectionVm>
{
    public GetCardRequestCollectionQueryHandler(IApplicationDbContext dbContext, IMapper mapper)
        : base(dbContext, mapper)
    {
    }

    public async Task<CardRequestCollectionVm> Handle(GetCardRequestCollectionQuery request,
        CancellationToken cancellationToken)
    {
        var query = DbContext.CardRequests.AsQueryable();

        if (!string.IsNullOrEmpty(request.SearchIn) && !string.IsNullOrEmpty(request.SearchQuery))
            query = request.SearchIn switch
            {
                nameof(CardRequest.CustomerName) => query.Where(item => item.CustomerName == request.SearchQuery),
                nameof(CardRequest.TargetAddress) => query.Where(item => item.TargetAddress == request.SearchQuery),
                nameof(CardRequest.PatientFullName) => query.Where(item => item.PatientFullName == request.SearchQuery),
                nameof(CardRequest.Status) => query.Where(item => item.Status.ToString() == request.SearchQuery),
                nameof(CardRequest.Priority) => query.Where(item => item.Priority.ToString() == request.SearchQuery),
                _ => query
            };

        query = request.OrderBy == OrderBy.Descending
            ? query.OrderByDescending(x => x.CreatedAt)
            : query.OrderBy(x => x.CreatedAt);

        var totalItems = await query.CountAsync(cancellationToken);
        var items = await query
            .Skip((int)request.Page * (int)request.Count)
            .Take((int)request.Count)
            .ToListAsync(cancellationToken);

        return Mapper.Map<CardRequestCollectionVm>(
            new VmCollection<CardRequestVm>
            {
                TotalCount = (uint)totalItems,
                Collection = Mapper.Map<ObservableCollection<CardRequestVm>>(items)
            });
    }
}
