using System.Runtime.Serialization;
using Budget.Domain.Counterparties;
using Newtonsoft.Json;

namespace Budget.Domain.Converters.EntityIdConverters;

public class CounterpartyIdConverter : JsonConverter
{
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        var id = (CounterpartyId)value;
        serializer.Serialize(writer, id.Value.ToString());
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        var guid = serializer.Deserialize<string>(reader) ??
                   throw new SerializationException($"Missing property {nameof(CounterpartyId.Value)} in {nameof(CounterpartyId)}"); ;

        return new CounterpartyId(guid);
    }

    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(CounterpartyId);
    }
}