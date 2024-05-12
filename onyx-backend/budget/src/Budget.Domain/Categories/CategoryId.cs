using Abstractions.DomainBaseTypes;
using Budget.Domain.Converters.EntityIdConverters;
using Newtonsoft.Json;

namespace Budget.Domain.Categories;

[JsonConverter(typeof(CategoryIdConverter))]
public sealed record CategoryId : EntityId
{
    public CategoryId() : base()
    {
        
    }

    public CategoryId(Guid value) : base(value)
    {
        
    }

    public CategoryId(string value) : base(value) 
    {
        
    }
}