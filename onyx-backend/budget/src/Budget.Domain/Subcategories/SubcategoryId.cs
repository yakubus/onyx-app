using Abstractions.DomainBaseTypes;
using Budget.Domain.Converters.EntityIdConverters;
using Newtonsoft.Json;

namespace Budget.Domain.Subcategories;

[JsonConverter(typeof(SubcategoryIdConverter))]
public sealed record SubcategoryId : EntityId
{
    public SubcategoryId() : base() { }

    public SubcategoryId(string value) : base(value) { }

    public SubcategoryId(Guid value) : base(value) { }
}