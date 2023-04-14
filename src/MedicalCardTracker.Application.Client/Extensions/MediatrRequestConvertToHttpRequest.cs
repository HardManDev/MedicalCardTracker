// Copyright (c) 2023 Mikulchik Vladislav Alekseevich <hardman.dev@pm.me>.
// This software is licensed under the MIT license.
// Please see the LICENSE file for more information.

using System.Net.Http.Json;
using System.Reflection;
using System.Web;
using MediatR;
using MedicalCardTracker.Application.Attributes;

namespace MedicalCardTracker.Application.Client.Extensions;

public static class MediatrRequestConvertToHttpRequest
{
    public static HttpRequestMessage ConvertToHttpRequest<TResponse>(this IRequest<TResponse> request,
        HttpMethod httpMethod,
        string endpoint)
        where TResponse : class
    {
        var requestProperties = request.GetType().GetProperties();

        var queryParameters = requestProperties
            .Where(p => p.GetCustomAttribute<QueryParameterAttribute>() != null)
            .Select(p => new KeyValuePair<string, string?>(p.Name, p.GetValue(request)?.ToString()));

        var requestBodyProperties = requestProperties
            .Where(p => p.GetCustomAttribute<QueryParameterAttribute>() == null)
            .ToDictionary(p => p.Name, p => p.GetValue(request));

        return new HttpRequestMessage
        {
            Method = httpMethod,
            RequestUri = new UriBuilder(endpoint)
            {
                Query = ToQueryString(queryParameters)
            }.Uri,
            Content = JsonContent.Create(requestBodyProperties)
        };
    }

    private static string ToQueryString(IEnumerable<KeyValuePair<string, string?>> pairs)
    {
        var encodedPairs = pairs
            .Where(pair => !string.IsNullOrEmpty(pair.Value))
            .Select(pair => $"{HttpUtility.UrlEncode(pair.Key)}={HttpUtility.UrlEncode(pair.Value)}");

        return string.Join("&", encodedPairs);
    }
}
