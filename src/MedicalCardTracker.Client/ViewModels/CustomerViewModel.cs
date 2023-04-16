// Copyright (c) 2023 Mikulchik Vladislav Alekseevich <hardman.dev@pm.me>.
// This software is licensed under the MIT license.
// Please see the LICENSE file for more information.

using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using MedicalCardTracker.Application.Client.Configuration;
using MedicalCardTracker.Application.Requests.Commands.CardRequests.CreateCardRequest;
using MedicalCardTracker.Client.Models.Enums;
using MedicalCardTracker.Client.Utils;
using MedicalCardTracker.Domain.Enums;
using Serilog;

namespace MedicalCardTracker.Client.ViewModels;

public class CustomerViewModel : BaseViewModel
{
    private string _description = string.Empty;
    private bool _isWindowEnable = true;
    private string _patientBirthDate = string.Empty;
    private string _patientFullName = string.Empty;
    private int _priority = (int)CardRequestPriority.Urgently;
    private RequestSendingProgress _requestSendingProgress = RequestSendingProgress.None;

    public CustomerViewModel(IMapper mapper, IMediator mediator, ApplicationConfiguration configuration)
        : base(mapper, mediator, configuration)
    {
        SendRequestCommand = new RelayCommand(SendRequestCommand_Execute, SendRequestCommand_CanExecute);
    }

    public string PatientFullName
    {
        get => _patientFullName;
        set
        {
            if (value == _patientFullName) return;
            _patientFullName = value ?? throw new ArgumentNullException(nameof(value));
            OnPropertyChanged();
        }
    }

    public string PatientBirthDate
    {
        get => _patientBirthDate;
        set
        {
            if (value == _patientBirthDate) return;
            _patientBirthDate = value ?? throw new ArgumentNullException(nameof(value));
            OnPropertyChanged();
        }
    }

    public string Description
    {
        get => _description;
        set
        {
            if (value == _description) return;
            _description = value ?? throw new ArgumentNullException(nameof(value));
            OnPropertyChanged();
        }
    }

    public int Priority
    {
        get => _priority;
        set
        {
            if (value == _priority) return;
            _priority = value;
            OnPropertyChanged();
        }
    }

    public bool IsWindowEnable
    {
        get => _isWindowEnable;
        set
        {
            if (value == _isWindowEnable) return;
            _isWindowEnable = value;
            OnPropertyChanged();
        }
    }

    public RequestSendingProgress RequestSendingProgress
    {
        get => _requestSendingProgress;
        set
        {
            if (value == _requestSendingProgress) return;
            _requestSendingProgress = value;
            OnPropertyChanged();
        }
    }

    public RelayCommand SendRequestCommand { get; }

    private bool SendRequestCommand_CanExecute(object obj)
    {
        return !string.IsNullOrEmpty(Configuration.CustomerName.Trim()) &&
               !string.IsNullOrEmpty(Configuration.TargetAddress.Trim()) &&
               !string.IsNullOrEmpty(PatientFullName.Trim()) &&
               !string.IsNullOrEmpty(PatientBirthDate.Trim()) &&
               DateTime.TryParse(PatientBirthDate.Trim(), out var patientBirthDate);
    }

    private async void SendRequestCommand_Execute(object obj)
    {
        try
        {
            IsWindowEnable = false;
            RequestSendingProgress = RequestSendingProgress.Pending;

            var priority = (CardRequestPriority)Priority;

            if (priority == 0)
                priority = CardRequestPriority.UnUrgently;

            await Mediator.Send(
                new CreateCardRequestCommand
                {
                    CustomerName = Configuration.CustomerName.Trim(),
                    TargetAddress = Configuration.TargetAddress.Trim(),
                    PatientFullName = PatientFullName.Trim(),
                    PatientBirthDate = DateOnly.FromDateTime(DateTime.Parse(PatientBirthDate.Trim())),
                    Description = Description.Trim(),
                    Priority = priority
                }, CancellationToken.None);

            RequestSendingProgress = RequestSendingProgress.Success;

            PatientFullName = string.Empty;
            PatientBirthDate = string.Empty;
            Description = string.Empty;
            Priority = (int)CardRequestPriority.Urgently;
        }
        catch (Exception e)
        {
            Log.Error("Failed send request", e);
            RequestSendingProgress = RequestSendingProgress.Failed;
        }
        finally
        {
            IsWindowEnable = true;
            await Task.Delay(5000);
            RequestSendingProgress = RequestSendingProgress.None;
        }
    }
}
