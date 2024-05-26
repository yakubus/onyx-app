using Newtonsoft.Json;

namespace Converters.DateTime;

public sealed class DateTimeConverter : JsonConverter
{
    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        if (value is null)
        {
            serializer.Serialize(writer, null);
            return;
        }

        var dateTime = (System.DateTime)value;
        dateTime = dateTime.ToUniversalTime();

        serializer.Serialize(writer, new DateObject(
            dateTime.Day,
            dateTime.Month,
            dateTime.Year,
            dateTime.Hour, 
            dateTime.Minute,
            dateTime.Second));
    }

    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        var dateObject = serializer.Deserialize<DateObject>(reader);

        if(dateObject is null)
        {
            return null;
        }

        return new System.DateTime(
            dateObject.Year,
            dateObject.Month,
            dateObject.Day,
            dateObject.Hour,
            dateObject.Minute,
            dateObject.Second,
            DateTimeKind.Utc);
    }

    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(System.DateTime);
    }

    private sealed record DateObject(
        int Day,
        int Month,
        int Year,
        int Hour,
        int Minute,
        int Second);
}