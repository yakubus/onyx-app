using Abstractions.DomainBaseTypes;

namespace Budget.Domain.Categories;

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