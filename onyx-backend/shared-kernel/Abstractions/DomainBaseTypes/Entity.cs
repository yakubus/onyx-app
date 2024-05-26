using System.Text.Json.Serialization;
using Abstractions.Messaging;
using Newtonsoft.Json;

namespace Abstractions.DomainBaseTypes;

public abstract class Entity<TEntityId> : IEntity where TEntityId : EntityId, new()
{
    [JsonProperty("id")]
    [JsonPropertyName("id")]
    public TEntityId Id { get; protected set; }

    private readonly List<IDomainEvent> _domainEvents = new();

    protected Entity()
    {
        Id = new TEntityId();
    }

    protected Entity(TEntityId id)
    {
        Id = id;
        _domainEvents = new();
    }

    [System.Text.Json.Serialization.JsonConstructor]
    [Newtonsoft.Json.JsonConstructor]
    private Entity(TEntityId id, List<IDomainEvent> domainEvents)
    {
        Id = id;
        _domainEvents = domainEvents;
    }

    public IReadOnlyList<IDomainEvent> GetDomainEvents() =>
        _domainEvents;

    public void ClearDomainEvents() =>
        _domainEvents.Clear();

    protected void RaiseDomainEvent(IDomainEvent domainEvent) =>
        _domainEvents.Add(domainEvent);
}