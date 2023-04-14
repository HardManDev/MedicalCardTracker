// Copyright (c) 2023 Mikulchik Vladislav Alekseevich <hardman.dev@pm.me>.
// This software is licensed under the MIT license.
// Please see the LICENSE file for more information.

using AutoMapper;
using MedicalCardTracker.Application.Interfaces;
using MedicalCardTracker.Application.Mappings;
using MedicalCardTracker.Database;
using MedicalCardTracker.Tests.Database;
using Xunit;

namespace MedicalCardTracker.Server.Tests.Fixtures;

public class CardRequestFixture : IDisposable
{
    public CardRequestFixture()
    {
        DbContext = ApplicationDbContextFactory.Create();
        var configurationProvider = new MapperConfiguration(config =>
        {
            config.AddProfile(new AssemblyMappingProfile(typeof(IMapWith<>).Assembly));
        });
        Mapper = configurationProvider.CreateMapper();
    }

    public ApplicationDbContext DbContext { get; }
    public IMapper Mapper { get; }

    public void Dispose() => ApplicationDbContextFactory.Destroy(DbContext);
}

[CollectionDefinition("CardRequestCollection")]
public class CardRequestCollection : ICollectionFixture<CardRequestFixture>
{
}
