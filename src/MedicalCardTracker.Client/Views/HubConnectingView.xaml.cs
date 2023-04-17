// Copyright (c) 2023 Mikulchik Vladislav Alekseevich <hardman.dev@pm.me>.
// This software is licensed under the MIT license.
// Please see the LICENSE file for more information.

using System.ComponentModel;
using System.Windows;
using MedicalCardTracker.Client.ViewModels;

namespace MedicalCardTracker.Client.Views;

public partial class HubConnectingView : Window
{
    public HubConnectingView(HubConnectingViewModel hubConnectingViewModel)
    {
        DataContext = hubConnectingViewModel;
        InitializeComponent();
    }

    private void HubConnectingView_OnClosing(object? sender, CancelEventArgs e)
    {
        System.Windows.Application.Current.Shutdown();
    }
}
