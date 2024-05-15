using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Identity.Domain;

public class UserIdConverter : JsonConverter
{
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        var id = (UserId)value;
        serializer.Serialize(writer, id.Value.ToString());
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        var guid = serializer.Deserialize<string>(reader) ??
                   throw new SerializationException($"Missing property {nameof(UserId.Value)} in {nameof(UserId)}");
        ;

        return new UserId(guid);
    }

    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(UserId);
    }
}