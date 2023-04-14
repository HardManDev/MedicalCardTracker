// Copyright (c) 2023 Mikulchik Vladislav Alekseevich <hardman.dev@pm.me>.
// This software is licensed under the MIT license.
// Please see the LICENSE file for more information.

using System.Text.Json;
using FluentAssertions;
using MedicalCardTracker.Application.Client.Extensions;
using MedicalCardTracker.Application.Requests.Commands.CardRequests.UpdateCardRequest;
using MedicalCardTracker.Domain.Enums;
using Xunit;

namespace MedicalCardTracker.Client.Tests.Extensions;

public class MediatrRequestConvertToHttpRequestTests
{
    [Fact]
    public void MediatrRequestConvertToHttpRequest_Success()
    {
        // Arrange
        var request = new UpdateCardRequestCommand
        {
            Id = Guid.NewGuid(),
            CustomerName = "Ivanov K. I.",
            TargetAddress = "cab. 100",
            PatientFullName = "Ivanov Roman Andreevich",
            PatientBirthDate = new DateOnly(2000, 10, 5),
            Description = null,
            Status = CardRequestStatus.Created,
            Priority = CardRequestPriority.Urgently
        };

        var expectedJson = JsonSerializer.Serialize(new
        {
            request.CustomerName,
            request.TargetAddress,
            request.PatientFullName,
            request.PatientBirthDate,
            request.Description,
            request.Status,
            request.Priority
        });

        // Act
        var result = request.ConvertToHttpRequest(
            HttpMethod.Patch,
            "api/CardRequest/Update");

        // Assert
        result.Should().BeOfType<HttpRequestMessage>();
        result.Method.Should().Be(HttpMethod.Patch);
        result.RequestUri.Should().Be(new Uri($"http://api/CardRequest/Update?Id={request.Id}"));
        result.Content.Should().NotBeNull();
        result.Content!.ReadAsStringAsync().Result.Should().Be(expectedJson);
    }
}
