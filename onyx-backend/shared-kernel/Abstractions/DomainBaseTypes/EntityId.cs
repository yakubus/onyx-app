using Models.Exceptions;

namespace Abstractions.DomainBaseTypes;

public abstract record EntityId : ValueObject
{
    public Guid Value { get; init; }

    protected internal EntityId(Guid value) => Value = value;

    protected internal EntityId() : this(Guid.NewGuid()) { }

    protected internal EntityId(string value)
    {
        if (!Guid.TryParse(value, out var id))
        {
            throw new DomainException<EntityId>("Invalid Id format");
        }

        Value = id;
    }
}