// Copyright (c) 2023 Mikulchik Vladislav Alekseevich <hardman.dev@pm.me>.
// This software is licensed under the MIT license.
// Please see the LICENSE file for more information.

using AutoMapper;
using MedicalCardTracker.Application.Server.Interfaces;
using MedicalCardTracker.Domain.Entities;
using MedicalCardTracker.Server.Tests.Fixtures;
using Microsoft.EntityFrameworkCore;

namespace MedicalCardTracker.Server.Tests.Requests;

public abstract class BaseRequestHandler
{
    protected BaseRequestHandler(IApplicationDbContext dbContext, IMapper mapper)
    {
        Mapper = mapper;
        DbContext = dbContext;

        ((DbContext)DbContext).Database.EnsureCreated();
        DbContext.CardRequests.AddRange(new List<CardRequest>
        {
            FixtureCardRequests.FixtureCardRequest,
            FixtureCardRequests.FixtureCardRequestForDelete,
            FixtureCardRequests.FixtureCardRequestForUpdate
        });
        ((DbContext)DbContext).SaveChanges();
    }

    protected IApplicationDbContext DbContext { get; }
    protected IMapper Mapper { get; }
}
