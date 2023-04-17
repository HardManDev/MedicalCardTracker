// Copyright (c) 2023 Mikulchik Vladislav Alekseevich <hardman.dev@pm.me>.
// This software is licensed under the MIT license.
// Please see the LICENSE file for more information.

using System.ComponentModel;
using System.Windows;
using MedicalCardTracker.Client.ViewModels;

namespace MedicalCardTracker.Client.Views;

public partial class CardRequestsView : Window
{
    public CardRequestsView(CardRequestsViewModel cardRequestsViewModel)
    {
        DataContext = cardRequestsViewModel;
        InitializeComponent();
    }

    private void CardRequestsView_OnClosing(object? sender, CancelEventArgs e)
    {
        e.Cancel = true;
        Hide();
    }
}
