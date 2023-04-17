// Copyright (c) 2023 Mikulchik Vladislav Alekseevich <hardman.dev@pm.me>.
// This software is licensed under the MIT license.
// Please see the LICENSE file for more information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using MedicalCardTracker.Application.Client.Configuration;
using MedicalCardTracker.Client.Models.Enums;
using Microsoft.AspNetCore.SignalR.Client;
using Serilog;

namespace MedicalCardTracker.Client.Utils;

public class HubConnectionHelper : INotifyPropertyChanged
{
    private readonly ApplicationConfiguration _configuration;
    private HubConnectionStatus _hubConnectionStatus = HubConnectionStatus.Connecting;

    public HubConnectionHelper(ApplicationConfiguration configuration)
    {
        _configuration = configuration;
        NotificationHubConnection = new HubConnectionBuilder()
            .WithUrl($"{configuration.ApiBaseUrl}/notifications")
            .Build();

        NotificationHubConnection.Closed += async e =>
        {
            await System.Windows.Application.Current.Dispatcher.Invoke(async () =>
            {
                HubConnectionStatus = HubConnectionStatus.Disconnected;

                await ReconnectToNotificationHub();
            });
        };
    }

    public HubConnection NotificationHubConnection { get; }

    public HubConnectionStatus HubConnectionStatus
    {
        get => _hubConnectionStatus;
        set
        {
            if (value == _hubConnectionStatus) return;
            _hubConnectionStatus = value;
            OnPropertyChanged();
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public async Task ConnectToNotificationHub()
    {
        try
        {
            await NotificationHubConnection.StartAsync();

            HubConnectionStatus = HubConnectionStatus.Connected;
        }
        catch (Exception e)
        {
            Log.Error("Failed connection to notification hub", e);

            HubConnectionStatus = HubConnectionStatus.Failed;

            string? messageBoxText = null;
            string? messageBoxCaption = null;

            try
            {
                messageBoxText =
                    (string?)System.Windows.Application.Current.Resources["NotificationHubConnectionError"];
                messageBoxCaption = (string?)(_configuration.IsRegistrar
                    ? System.Windows.Application.Current.Resources["RegistrarViewTitle"]
                    : System.Windows.Application.Current.Resources["CustomerViewTitle"]);
            }
            catch (Exception)
            {
                // ignore
            }

            var msgBoxResult = MessageBox.Show(
                messageBoxText ?? "Failed connection to notification hub",
                messageBoxCaption ?? "MedicalCardTracker.Client",
                MessageBoxButton.OK,
                MessageBoxImage.Error);

            if (msgBoxResult == MessageBoxResult.OK)
                System.Windows.Application.Current.Shutdown();
        }
    }

    private async Task ReconnectToNotificationHub()
    {
        try
        {
            HubConnectionStatus = HubConnectionStatus.Reconnecting;

            await Task.Delay(5000);
            await NotificationHubConnection.StartAsync();

            HubConnectionStatus = HubConnectionStatus.Connected;
        }
        catch (Exception e)
        {
            Log.Error("Failed reconnection to notification hub", e);

            await ReconnectToNotificationHub();
        }
    }

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}
