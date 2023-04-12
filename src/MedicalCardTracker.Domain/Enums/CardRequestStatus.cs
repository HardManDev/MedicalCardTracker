// Copyright (c) 2023 Mikulchik Vladislav Alekseevich <hardman.dev@pm.me>.
// This software is licensed under the MIT license.
// Please see the LICENSE file for more information.

namespace MedicalCardTracker.Domain.Enums;

public enum CardRequestStatus
{
    Canceled = -2,
    NotCompleted = -1,
    Created = 0,
    Completed = 1
}
