// Copyright (c) 2023 Mikulchik Vladislav Alekseevich <hardman.dev@pm.me>.
// This software is licensed under the MIT license.
// Please see the LICENSE file for more information.

using System;
using System.Globalization;
using System.Windows.Data;
using MedicalCardTracker.Domain.Enums;

namespace MedicalCardTracker.Client.Convertors;

public class PriorityToStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not CardRequestPriority urgency) return string.Empty;

        return urgency switch
        {
            CardRequestPriority.Urgently => "Срочно",
            CardRequestPriority.UnUrgently => "Не срочно",
            _ => string.Empty
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
