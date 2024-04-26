using Abstractions.DomainBaseTypes;

namespace Budget.Domain.Subcategories;

public sealed record SubcategoryId : EntityId
{
    public SubcategoryId() : base() { }

    public SubcategoryId(string value) : base(value) { }

    public SubcategoryId(Guid value) : base(value) { }
}