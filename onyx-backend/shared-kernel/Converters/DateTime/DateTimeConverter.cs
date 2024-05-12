using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Converters.DateTime;

public sealed class DateTimeConverter : JsonConverter<System.DateTime>
{
    public override System.DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
        {
            var dateString = reader.GetString();
            if (System.DateTime.TryParseExact(
                    dateString,
                    "dd-MM-yyyyTHH:mm:ss",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out System.DateTime result))
            {
                return result.ToUniversalTime();
            }
        }

        try
        {
            return reader.GetDateTime().ToUniversalTime();
        }
        catch (FormatException)
        {
            return System.DateTime.ParseExact(reader.GetString(), "dd-MM-yyyyTHH:mm:ss", CultureInfo.InvariantCulture);
        }
    }

    public override void Write(Utf8JsonWriter writer, System.DateTime value, JsonSerializerOptions options)
    {
        var localTime = value.ToLocalTime();

        writer.WriteStringValue(localTime.ToString("dd-MM-yyyyTHH:mm:ss"));
    }
}