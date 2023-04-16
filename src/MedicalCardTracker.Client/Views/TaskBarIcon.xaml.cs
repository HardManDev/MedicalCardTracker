// Copyright (c) 2023 Mikulchik Vladislav Alekseevich <hardman.dev@pm.me>.
// This software is licensed under the MIT license.
// Please see the LICENSE file for more information.

using System.Reflection;
using System.Windows;
using MedicalCardTracker.Client.ViewModels;

namespace MedicalCardTracker.Client.Views;

public partial class TaskBarIcon : Window
{
    public TaskBarIcon(TaskBarIconViewModel taskBarIconVm)
    {
        DataContext = taskBarIconVm;
        InitializeComponent();
        NotifyIcon.Icon = System.Drawing.Icon.ExtractAssociatedIcon(Assembly.GetExecutingAssembly().Location);
    }
}
