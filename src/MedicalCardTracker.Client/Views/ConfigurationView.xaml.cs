// Copyright (c) 2023 Mikulchik Vladislav Alekseevich <hardman.dev@pm.me>.
// This software is licensed under the MIT license.
// Please see the LICENSE file for more information.

using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using MedicalCardTracker.Client.ViewModels;

namespace MedicalCardTracker.Client.Views;

public partial class ConfigurationView : Window
{
    private readonly ConfigurationViewModel _configurationViewModel;

    public ConfigurationView(ConfigurationViewModel configurationViewModel)
    {
        _configurationViewModel = configurationViewModel;

        DataContext = _configurationViewModel;
        InitializeComponent();
    }

    private void ConfigurationView_OnClosing(object? sender, CancelEventArgs e)
    {
        e.Cancel = true;
        Hide();
    }

    private void ConfigurationView_OnMouseRightButtonUp(object sender, MouseButtonEventArgs e)
    {
        _configurationViewModel.IsEditable = !_configurationViewModel.IsEditable;
    }
}
