// Copyright (c) 2023 Mikulchik Vladislav Alekseevich <hardman.dev@pm.me>.
// This software is licensed under the MIT license.
// Please see the LICENSE file for more information.

using System.Net.Http.Json;
using MediatR;
using MedicalCardTracker.Application.Client.Extensions;

namespace MedicalCardTracker.Application.Client.Requests;

public abstract class BaseRequestHandler
{
    protected BaseRequestHandler(HttpClient httpClient)
        => HttpClient = httpClient;

    protected HttpClient HttpClient { get; }

    protected async Task<TResponse> SendHttpRequest<TResponse>(IRequest<TResponse> request,
        HttpMethod httpMethod,
        string endpoint,
        CancellationToken cancellationToken = default)
    {
        var httpRequest = request.ConvertToHttpRequest(httpMethod, $"{HttpClient.BaseAddress}{endpoint}");

        var httpResponse = await HttpClient.SendAsync(httpRequest, cancellationToken);

        if (httpResponse.IsSuccessStatusCode)
        {
            var result =
                await httpResponse.Content
                    .ReadFromJsonAsync<TResponse>(cancellationToken: cancellationToken)
                ?? throw new HttpRequestException($"Failed to read response as JSON with T:{typeof(TResponse)}.");

            return result;
        }

        var content = await httpResponse.Content.ReadAsStringAsync(cancellationToken);

        throw new HttpRequestException(
            $"HTTP request failed with status code {httpResponse.StatusCode}. Error message: {content}");
    }
}
