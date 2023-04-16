// Copyright (c) 2023 Mikulchik Vladislav Alekseevich <hardman.dev@pm.me>.
// This software is licensed under the MIT license.
// Please see the LICENSE file for more information.

namespace MedicalCardTracker.Client.Models.Enums;

public enum HubConnectionStatus
{
    Reconnecting = -3,
    Disconnected = -2,
    Failed = -1,
    Connecting = 0,
    Connected = 1
}
