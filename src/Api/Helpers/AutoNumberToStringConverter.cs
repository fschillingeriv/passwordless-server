﻿using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Passwordless.Api.Helpers;

public class AutoNumberToStringConverter : JsonConverter<string?>
{
    public override string? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Number)
        {
            if (reader.TryGetInt64(out long number))
            {
                return number.ToString(CultureInfo.InvariantCulture);
            }

            if (reader.TryGetDouble(out var doubleNumber))
            {
                return doubleNumber.ToString(CultureInfo.InvariantCulture);
            }
        }

        if (reader.TokenType == JsonTokenType.String)
        {
            return reader.GetString();
        }

        using var document = JsonDocument.ParseValue(ref reader);
        return document.RootElement.Clone().ToString();
    }

    public override void Write(Utf8JsonWriter writer, string? value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value);
    }
}