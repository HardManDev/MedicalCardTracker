// Copyright (c) 2023 Mikulchik Vladislav Alekseevich <hardman.dev@pm.me>.
// This software is licensed under the MIT license.
// Please see the LICENSE file for more information.

using AutoMapper;
using MedicalCardTracker.Database;
using MedicalCardTracker.Server.Tests.Fixtures;

namespace MedicalCardTracker.Server.Tests.Requests;

public abstract class BaseRequestHandler
{
    protected BaseRequestHandler(CardRequestFixture fixture)
        => (DbContext, Mapper) = (fixture.DbContext, fixture.Mapper);

    protected ApplicationDbContext DbContext { get; }
    protected IMapper Mapper { get; }
}
