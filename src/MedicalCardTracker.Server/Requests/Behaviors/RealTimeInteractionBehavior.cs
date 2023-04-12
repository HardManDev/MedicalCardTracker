// Copyright (c) 2023 Mikulchik Vladislav Alekseevich <hardman.dev@pm.me>.
// This software is licensed under the MIT license.
// Please see the LICENSE file for more information.

using MediatR;
using MedicalCardTracker.Server.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace MedicalCardTracker.Server.Requests.Behaviors;

public class RealTimeInteractionBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse> where TRequest
    : IRequest<TResponse>
{
    private readonly IHubContext<NotificationHub> _notificationHubContext;

    public RealTimeInteractionBehavior(IHubContext<NotificationHub> notificationHubContext)
        => _notificationHubContext = notificationHubContext;

    public async Task<TResponse> Handle(TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var result = await next();
        var requestName = typeof(TRequest).Name;

        await _notificationHubContext.Clients.All
            .SendAsync($"On{requestName}", "Server", result, cancellationToken);

        return result;
    }
}
