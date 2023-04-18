// Copyright (c) 2023 Mikulchik Vladislav Alekseevich <hardman.dev@pm.me>.
// This software is licensed under the MIT license.
// Please see the LICENSE file for more information.

using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using AutoMapper;
using MediatR;
using MedicalCardTracker.Application.Client.Configuration;
using MedicalCardTracker.Client.Utils;

namespace MedicalCardTracker.Client.ViewModels;

public class ConfigurationViewModel : BaseViewModel
{
    private bool _isEditable;

    public ConfigurationViewModel(IMapper mapper, IMediator mediator, ApplicationConfiguration configuration)
        : base(mapper, mediator, configuration)
    {
        Configuration.PropertyChanged += async (sender, args) => { await Configuration.SaveToJsonAsync(); };

        OpenCurrentDirectoryCommand = new RelayCommand(OpenCurrentDirectoryCommand_Execute, o => true);
        RegistrarSwitchCommand = new RelayCommand(RegistrarSwitchCommand_Execute, o => IsEditable);
    }

    public bool IsEditable
    {
        get => _isEditable;
        set
        {
            if (value == _isEditable) return;
            _isEditable = value;
            OnPropertyChanged();
        }
    }

    public RelayCommand OpenCurrentDirectoryCommand { get; }
    public RelayCommand RegistrarSwitchCommand { get; }

    private void RegistrarSwitchCommand_Execute(object obj)
    {
        var result = MessageBox.Show(
            (string)System.Windows.Application.Current.Resources["RegistrarModeWarning"],
            (string)System.Windows.Application.Current.Resources["RegistrarMode"],
            MessageBoxButtons.OK,
            MessageBoxIcon.Warning);

        if (result != DialogResult.OK) return;

        Configuration.IsRegistrar = !Configuration.IsRegistrar;
        System.Windows.Application.Current.Shutdown();
    }

    private void OpenCurrentDirectoryCommand_Execute(object obj)
    {
        Process.Start("explorer.exe", "\"" + Directory.GetCurrentDirectory() + "\"");
    }
}
