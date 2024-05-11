using System.Runtime.Serialization;
using System.Transactions;
using Budget.Domain.Transactions;
using Newtonsoft.Json;

namespace Budget.Domain.Converters.EntityIdConverters;

public class TransactionIdConverter : JsonConverter
{
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        var id = (TransactionId)value;
        serializer.Serialize(writer, id.Value.ToString());
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        var guid = serializer.Deserialize<string>(reader) ??
                   throw new SerializationException($"Missing property {nameof(TransactionId.Value)} in {nameof(TransactionId)}"); ;

        return new TransactionId(guid);
    }

    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(TransactionId);
    }
}