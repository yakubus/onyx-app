using System.Text.Json.Serialization;
using Abstractions.Messaging;
using Newtonsoft.Json;

namespace Abstractions.DomainBaseTypes;

public abstract class Entity<TEntityId> : IEntity where TEntityId : EntityId
{
    [JsonProperty("id")]
    [JsonPropertyName("id")]
    public TEntityId Id { get; protected set; }

    private readonly List<IDomainEvent> _domainEvents = new();

    protected Entity() 
    { }

    protected Entity(TEntityId id)
    {
        this.Id = id;
        _domainEvents = new();
    }

    public IReadOnlyList<IDomainEvent> GetDomainEvents() =>
        _domainEvents;

    public void ClearDomainEvents() =>
        _domainEvents.Clear();

    protected void RaiseDomainEvent(IDomainEvent domainEvent) =>
        _domainEvents.Add(domainEvent);
}