using System.Runtime.Serialization;
using Budget.Domain.Categories;
using Newtonsoft.Json;

namespace Budget.Domain.Converters.EntityIdConverters;

public class CategoryIdConverter : JsonConverter
{
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        var id = (CategoryId)value;
        serializer.Serialize(writer, id.Value.ToString());
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        var guid = serializer.Deserialize<string>(reader) ??
                   throw new SerializationException($"Missing property {nameof(CategoryId.Value)} in {nameof(CategoryId)}"); ;

        return new CategoryId(guid);
    }

    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(CategoryId);
    }
}