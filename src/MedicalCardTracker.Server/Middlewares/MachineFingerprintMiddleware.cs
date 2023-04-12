// Copyright (c) 2023 Mikulchik Vladislav Alekseevich <hardman.dev@pm.me>.
// This software is licensed under the MIT license.
// Please see the LICENSE file for more information.

using System.Text.RegularExpressions;
using Serilog;

namespace MedicalCardTracker.Server.Middlewares;

public class MachineFingerprintMiddleware
{
    private readonly RequestDelegate _next;

    public MachineFingerprintMiddleware(RequestDelegate next)
        => _next = next;

    public async Task Invoke(HttpContext httpContext)
    {
        var machineMetadata = httpContext.Request.Headers["X-Machine-Fingerprint"].FirstOrDefault();

        if (machineMetadata != null)
        {
            var match = Regex.Match(machineMetadata,
                @"^(\w+)@(\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3})$");

            if (match.Success)
            {
                var hostname = match.Groups[1].Value;
                var ipAddress = match.Groups[2].Value;

                Log.Information("Executor machine info:" +
                                $"\n\t\t\t   [*] IP: {ipAddress}" +
                                $"\n\t\t\t   [*] HostName: {hostname}");
            }
            else
            {
                Log.Warning("Executor machine info: failed to parse.");
            }
        }
        else
        {
            Log.Warning("Executor machine info: null");
        }

        await _next(httpContext);
    }
}
