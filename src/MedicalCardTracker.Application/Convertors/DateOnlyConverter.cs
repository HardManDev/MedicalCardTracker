// Copyright (c) 2023 Mikulchik Vladislav Alekseevich <hardman.dev@pm.me>.
// This software is licensed under the MIT license.
// Please see the LICENSE file for more information.

using System.Text.Json;
using System.Text.Json.Serialization;

namespace MedicalCardTracker.Application.Convertors;

public class DateOnlyConverter : JsonConverter<DateOnly?>
{
    public override DateOnly? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var dateObj = JsonSerializer.Deserialize<DateOnlyObject>(ref reader, options);
        if (dateObj == null) return null;

        return new DateOnly(dateObj.Year, dateObj.Month, dateObj.Day);
    }

    public override void Write(Utf8JsonWriter writer, DateOnly? value, JsonSerializerOptions options)
    {
        if (value.HasValue)
            JsonSerializer.Serialize(writer, new DateOnlyObject
            {
                Year = value.Value.Year,
                Month = value.Value.Month,
                Day = value.Value.Day
            }, options);
        else
            writer.WriteNullValue();
    }

    private class DateOnlyObject
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
    }
}
