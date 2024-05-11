using System.Runtime.Serialization;
using Budget.Domain.Accounts;
using Newtonsoft.Json;

namespace Budget.Domain.Converters.EntityIdConverters;

public class AccountIdConverter : JsonConverter
{
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        var id = (AccountId)value;
        serializer.Serialize(writer, id.Value.ToString());
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        var guid = serializer.Deserialize<string>(reader) ??
                   throw new SerializationException($"Missing property {nameof(AccountId.Value)} in {nameof(AccountId)}"); ;

        return new AccountId(guid);
    }

    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(AccountId);
    }
}