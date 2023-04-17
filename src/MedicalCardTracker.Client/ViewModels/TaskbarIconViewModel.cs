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

public class TaskbarIconViewModel : BaseViewModel
{
    private readonly CardRequestsView _cardRequestsView;
    private readonly CustomerView _customerView;

    public TaskbarIconViewModel(IMapper mapper,
        IMediator mediator,
        ApplicationConfiguration configuration,
        CustomerView customerView,
        CardRequestsView cardRequestsView)
        : base(mapper, mediator, configuration)
    {
        _customerView = customerView;
        _cardRequestsView = cardRequestsView;

        ExitCommand = new RelayCommand(ExitCommand_Execute, o => true);
        OpenCustomerViewCommand = new RelayCommand(OpenCustomerViewCommand_Execute, o => true);
        OpenCardRequestsCommand = new RelayCommand(OpenCardRequestsCommand_Execute, o => true);
        OpenMainViewCommand = new RelayCommand(OpenMainViewCommand_Execute, o => true);
    }

    public RelayCommand ExitCommand { get; }
    public RelayCommand OpenCustomerViewCommand { get; }
    public RelayCommand OpenCardRequestsCommand { get; }
    public RelayCommand OpenMainViewCommand { get; }

    private void OpenMainViewCommand_Execute(object obj)
    {
        if (Configuration.IsRegistrar)
            _cardRequestsView.Show();
        else
            _customerView.Show();
    }

    private void OpenCardRequestsCommand_Execute(object obj)
    {
        _cardRequestsView.Show();
    }

    private void OpenCustomerViewCommand_Execute(object obj)
    {
        _customerView.Show();
    }

    private static void ExitCommand_Execute(object obj)
    {
        Log.Information("Application shutdown");
        System.Windows.Application.Current.Shutdown();
    }
}
