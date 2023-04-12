// Copyright (c) 2023 Mikulchik Vladislav Alekseevich <hardman.dev@pm.me>.
// This software is licensed under the MIT license.
// Please see the LICENSE file for more information.

using System.Collections.ObjectModel;

namespace MedicalCardTracker.Application.Models;

public class VmCollection<TCollection> where TCollection : class
{
    public uint TotalCount { get; set; }
    public ObservableCollection<TCollection> Collection { get; set; } = null!;
}
