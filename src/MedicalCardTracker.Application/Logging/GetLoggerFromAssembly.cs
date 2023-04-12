// Copyright (c) 2023 Mikulchik Vladislav Alekseevich <hardman.dev@pm.me>.
// This software is licensed under the MIT license.
// Please see the LICENSE file for more information.

using System.Reflection;
using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace MedicalCardTracker.Application.Logging;

public static class GetLoggerFromAssembly
{
    public static Logger GetLogger(this Assembly assembly)
    {
        return new LoggerConfiguration()
            .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command",
                LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.AspNetCore.Mvc.Infrastructure.ObjectResultExecutor",
                LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker",
                LogEventLevel.Warning)
            .WriteTo.Console(outputTemplate:
                "{Timestamp:dd.MM.yyyy HH:mm:ss} [{Level:u4}] {Message}{NewLine}{Exception}")
#if !DEBUG
            .WriteTo.File($"logs/.log",
                outputTemplate: "{Timestamp:dd.MM.yyyy HH:mm:ss} [{Level:u4}] ({SourceContext}) {Message}{NewLine}{Exception}",
                rollingInterval: RollingInterval.Day,
                shared: true,
                retainedFileCountLimit: 15)
#endif
            .CreateLogger();
    }
}
