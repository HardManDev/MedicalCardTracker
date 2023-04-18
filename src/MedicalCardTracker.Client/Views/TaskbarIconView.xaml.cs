// Copyright (c) 2023 Mikulchik Vladislav Alekseevich <hardman.dev@pm.me>.
// This software is licensed under the MIT license.
// Please see the LICENSE file for more information.

using System;
using System.Windows;
using Hardcodet.Wpf.TaskbarNotification;
using MedicalCardTracker.Application.Client.Configuration;
using MedicalCardTracker.Application.Models.ViewModels;
using MedicalCardTracker.Application.Requests.Commands.CardRequests.CreateCardRequest;
using MedicalCardTracker.Application.Requests.Commands.CardRequests.UpdateCardRequest;
using MedicalCardTracker.Client.Utils;
using MedicalCardTracker.Client.ViewModels;
using MedicalCardTracker.Domain.Enums;
using Microsoft.AspNetCore.SignalR.Client;

namespace MedicalCardTracker.Client.Views;

public partial class TaskbarIconView : Window
{
    private readonly ApplicationConfiguration _configuration;

    public TaskbarIconView(TaskbarIconViewModel taskbarIconViewModel,
        ApplicationConfiguration configuration,
        HubConnectionHelper hubConnectionHelper)
    {
        _configuration = configuration;

        hubConnectionHelper.NotificationHubConnection.On<string, CardRequestVm>(
            $"On{nameof(CreateCardRequestCommand)}", OnCreateCardRequestCommandHandler);
        hubConnectionHelper.NotificationHubConnection.On<string, CardRequestVm>(
            $"On{nameof(UpdateCardRequestCommand)}", OnUpdateCardRequestCommandHandler);

        DataContext = taskbarIconViewModel;
        InitializeComponent();

        TaskbarNotifyIcon.TrayBalloonTipClicked += (sender, args)
            => taskbarIconViewModel.OpenCardRequestsCommand.Execute(null);
    }

    private void OnCreateCardRequestCommandHandler(string user, CardRequestVm payload)
    {
        if (_configuration.IsRegistrar)
            TaskbarNotifyIcon.ShowBalloonTip(
                (string)System.Windows.Application.Current.Resources["NewCardRequestNotifyTitle"],
                $"{payload.PatientFullName} ({payload.PatientBirthDate}) ⇀ {payload.TargetAddress}",
                BalloonIcon.Info);
    }

    private void OnUpdateCardRequestCommandHandler(string user, CardRequestVm payload)
    {
        switch (payload.Status)
        {
            case CardRequestStatus.Canceled:
                if (_configuration.IsRegistrar)
                    TaskbarNotifyIcon.ShowBalloonTip(
                        (string)System.Windows.Application.Current.Resources["CardRequestCanceledNotifyTitle"],
                        $"{payload.PatientFullName} ({payload.PatientBirthDate}) ⇀ {payload.TargetAddress}",
                        BalloonIcon.Warning);
                break;
            case CardRequestStatus.NotCompleted:
                if (!_configuration.IsRegistrar && _configuration.TargetAddress == payload.TargetAddress)
                    TaskbarNotifyIcon.ShowBalloonTip(
                        (string)System.Windows.Application.Current.Resources["CardRequestNotCompletedNotifyTitle"],
                        $"{payload.PatientFullName} ({payload.PatientBirthDate})",
                        BalloonIcon.Error);
                break;
            case CardRequestStatus.Created:
                break;
            case CardRequestStatus.Completed:
                if (!_configuration.IsRegistrar && _configuration.TargetAddress == payload.TargetAddress)
                    TaskbarNotifyIcon.ShowBalloonTip(
                        (string)System.Windows.Application.Current.Resources["CardRequestCompletedNotifyTitle"],
                        $"{payload.PatientFullName} ({payload.PatientBirthDate})",
                        BalloonIcon.Info);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
