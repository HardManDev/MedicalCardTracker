// Copyright (c) 2023 Mikulchik Vladislav Alekseevich <hardman.dev@pm.me>.
// This software is licensed under the MIT license.
// Please see the LICENSE file for more information.

using MedicalCardTracker.Server;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace MedicalCardTracker.Client.Tests.Fixtures;

[CollectionDefinition("CardRequestCollection")]
public class CardRequestCollection : ICollectionFixture<WebApplicationFactory<Program>>
{
}
