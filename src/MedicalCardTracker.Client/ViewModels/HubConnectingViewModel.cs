// Copyright (c) 2023 Mikulchik Vladislav Alekseevich <hardman.dev@pm.me>.
// This software is licensed under the MIT license.
// Please see the LICENSE file for more information.

using System;
using AutoMapper;
using MediatR;
using MedicalCardTracker.Application.Client.Configuration;
using MedicalCardTracker.Client.Utils;

namespace MedicalCardTracker.Client.ViewModels;

public class HubConnectingViewModel : BaseViewModel
{
    private HubConnectionHelper _hubConnectionHelper;

    public HubConnectingViewModel(IMapper mapper,
        IMediator mediator,
        ApplicationConfiguration configuration,
        HubConnectionHelper hubConnectionHelper)
        : base(mapper, mediator, configuration)
    {
        _hubConnectionHelper = hubConnectionHelper;
    }

    public HubConnectionHelper HubConnectionHelper
    {
        get => _hubConnectionHelper;
        set
        {
            if (Equals(value, _hubConnectionHelper)) return;
            _hubConnectionHelper = value ?? throw new ArgumentNullException(nameof(value));
            OnPropertyChanged();
        }
    }
}
