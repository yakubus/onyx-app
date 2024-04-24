using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PennyPlanner.Domain.Shared
{
    public sealed class DateTimeConverter : JsonConverter<DateTime>
    {
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                var dateString = reader.GetString();
                if (DateTime.TryParseExact(dateString, "dd-MM-yyyyTHH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result))
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
                return DateTime.ParseExact(reader.GetString(), "dd-MM-yyyyTHH:mm:ss", CultureInfo.InvariantCulture);
            }
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            var localTime = value.ToLocalTime();

            writer.WriteStringValue(localTime.ToString("dd-MM-yyyyTHH:mm:ss"));
        }
    }
}
