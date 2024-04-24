using Models.Exceptions;

namespace Abstractions.DomainBaseTypes;

public abstract record EntityId
{
    public Guid Value { get; init; }

    protected EntityId() : this(Guid.NewGuid())
    {
    }

    protected EntityId(Guid value) => Value = value;

    protected EntityId(string value)
    {
        if (!Guid.TryParse(value, out var id))
        {
            throw new DomainException<EntityId>("Invalid id format");
        }

        Value = id;
    }
}