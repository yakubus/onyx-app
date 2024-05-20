using System.Runtime.Serialization;
using Budget.Domain.Accounts;
using Budget.Domain.Budgets;
using Newtonsoft.Json;

namespace Budget.Domain.Converters.EntityIdConverters;

public sealed class BudgetIdConverter : JsonConverter
{
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        var id = (BudgetId)value;
        serializer.Serialize(writer, id.Value.ToString());
    }

    public override object? ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        var guid = serializer.Deserialize<string>(reader);

        return guid is null ? null : new BudgetId(guid);
    }

    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(BudgetId);
    }
}