// Copyright (c) 2023 Mikulchik Vladislav Alekseevich <hardman.dev@pm.me>.
// This software is licensed under the MIT license.
// Please see the LICENSE file for more information.

namespace MedicalCardTracker.Application.Server.Exceptions;

public class EntityNotFoundException : Exception
{
    public EntityNotFoundException(string entityName, object entityKey)
        : base($"Entity \"{entityName}\" with key: \'{entityKey}\' not found.")
    {
    }
}
