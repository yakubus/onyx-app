using Budget.Domain.Accounts;
using Newtonsoft.Json;

namespace Budget.Domain.Converters.EntityIdConverters;

public class AccountIdConverter : JsonConverter
{
    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        var id = value as AccountId;
        serializer.Serialize(writer, id?.Value.ToString());
    }

    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        var guid = serializer.Deserialize<string>(reader);

        return guid is null ? null : new AccountId(guid);
    }

    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(AccountId);
    }
}