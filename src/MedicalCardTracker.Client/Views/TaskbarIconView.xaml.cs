// Copyright (c) 2023 Mikulchik Vladislav Alekseevich <hardman.dev@pm.me>.
// This software is licensed under the MIT license.
// Please see the LICENSE file for more information.

using System.Windows;
using MedicalCardTracker.Client.ViewModels;

namespace MedicalCardTracker.Client.Views;

public partial class TaskbarIconView : Window
{
    public TaskbarIconView(TaskbarIconViewModel taskbarIconViewModel)
    {
        DataContext = taskbarIconViewModel;
        InitializeComponent();
    }
}
