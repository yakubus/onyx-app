using Abstractions.Messaging;

namespace Abstractions.DomainBaseTypes;

public abstract class Entity<TEntityId> : IEntity where TEntityId : EntityId
{
    public TEntityId Id { get; protected set; }

    private readonly List<IDomainEvent> _domainEvents = new();

    protected Entity() { }

    protected Entity(TEntityId id)
    {
        Id = id;
        _domainEvents = new();
    }

    public IReadOnlyList<IDomainEvent> GetDomainEvents() =>
        _domainEvents;

    public void ClearDomainEvents() =>
        _domainEvents.Clear();

    protected void RaiseDomainEvent(IDomainEvent domainEvent) =>
        _domainEvents.Add(domainEvent);
}