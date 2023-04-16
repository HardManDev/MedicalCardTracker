// Copyright (c) 2023 Mikulchik Vladislav Alekseevich <hardman.dev@pm.me>.
// This software is licensed under the MIT license.
// Please see the LICENSE file for more information.

using AutoMapper;
using MediatR;
using MedicalCardTracker.Application.Client.Configuration;
using MedicalCardTracker.Client.Utils;
using MedicalCardTracker.Client.Views;
using Serilog;

namespace MedicalCardTracker.Client.ViewModels;

public class TaskBarIconViewModel : BaseViewModel
{
    private readonly CustomerView _customerView;

    public TaskBarIconViewModel(IMapper mapper,
        IMediator mediator,
        ApplicationConfiguration configuration,
        CustomerView customerView)
        : base(mapper, mediator, configuration)
    {
        _customerView = customerView;

        OpenViewCommand = new RelayCommand(OpenViewCommand_Execute, OpenViewCommand_CanExecute);
        ExitCommand = new RelayCommand(ExitCommand_Execute, ExitCommand_CanExecute);
    }

    public RelayCommand ExitCommand { get; }
    public RelayCommand OpenViewCommand { get; }

    private static bool ExitCommand_CanExecute(object arg) => true;

    private static void ExitCommand_Execute(object obj)
    {
        Log.Information("Application shutdown");
        System.Windows.Application.Current.Shutdown();
    }

    private static bool OpenViewCommand_CanExecute(object obj) => true;

    private void OpenViewCommand_Execute(object obj)
    {
        if (!Configuration.IsRegistrar)
            _customerView.Show();
    }
}
