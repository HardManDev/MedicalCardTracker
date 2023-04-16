// Copyright (c) 2023 Mikulchik Vladislav Alekseevich <hardman.dev@pm.me>.
// This software is licensed under the MIT license.
// Please see the LICENSE file for more information.

using MedicalCardTracker.Client.Utils;
using Serilog;

namespace MedicalCardTracker.Client.ViewModels;

public class TaskBarIconViewModel
{
    public TaskBarIconViewModel()
    {
        ExitCommand = new RelayCommand(ExitCommand_Execute, ExitCommand_CanExecute);
    }

    public RelayCommand ExitCommand { get; }

    private static bool ExitCommand_CanExecute(object arg) => true;

    private static void ExitCommand_Execute(object obj)
    {
        Log.Information("Application shutdown");
        System.Windows.Application.Current.Shutdown();
    }
}
