// Copyright (c) 2023 Mikulchik Vladislav Alekseevich <hardman.dev@pm.me>.
// This software is licensed under the MIT license.
// Please see the LICENSE file for more information.

using MedicalCardTracker.Server;

internal class Program
{
    public static void Main(string[] args)
        => new Application(args).Run();
}
