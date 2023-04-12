// Copyright (c) 2023 Mikulchik Vladislav Alekseevich <hardman.dev@pm.me>.
// This software is licensed under the MIT license.
// Please see the LICENSE file for more information.

using System.Reflection;
using MedicalCardTracker.Application.Logging;
using MedicalCardTracker.Server;
using Serilog;

internal class Program
{
    public static void Main(string[] args)
    {
        Log.Logger = Assembly.GetExecutingAssembly().GetLogger();

        new Application(args).Run();
    }
}
