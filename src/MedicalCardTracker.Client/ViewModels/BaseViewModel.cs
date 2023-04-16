// Copyright (c) 2023 Mikulchik Vladislav Alekseevich <hardman.dev@pm.me>.
// This software is licensed under the MIT license.
// Please see the LICENSE file for more information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using AutoMapper;
using MediatR;
using MedicalCardTracker.Application.Client.Configuration;

namespace MedicalCardTracker.Client.ViewModels;

public abstract class BaseViewModel : INotifyPropertyChanged
{
    private ApplicationConfiguration _configuration;

    protected BaseViewModel(IMapper mapper, IMediator mediator, ApplicationConfiguration configuration)
    {
        Mapper = mapper;
        Mediator = mediator;

        _configuration = configuration;
    }

    protected IMapper Mapper { get; }
    protected IMediator Mediator { get; }

    public ApplicationConfiguration Configuration
    {
        get => _configuration;
        set
        {
            if (Equals(value, _configuration)) return;
            _configuration = value ?? throw new ArgumentNullException(nameof(value));
            OnPropertyChanged();
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}
