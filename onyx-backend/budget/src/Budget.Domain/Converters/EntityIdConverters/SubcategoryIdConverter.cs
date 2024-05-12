using System.Runtime.Serialization;
using Budget.Domain.Subcategories;
using Newtonsoft.Json;

namespace Budget.Domain.Converters.EntityIdConverters;

public class SubcategoryIdConverter : JsonConverter
{
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        var id = (SubcategoryId)value;
        serializer.Serialize(writer, id.Value.ToString());
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        var guid = serializer.Deserialize<string>(reader) ??
                   throw new SerializationException($"Missing property {nameof(SubcategoryId.Value)} in {nameof(SubcategoryId)}"); ;

        return new SubcategoryId(guid);
    }

    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(SubcategoryId);
    }
}