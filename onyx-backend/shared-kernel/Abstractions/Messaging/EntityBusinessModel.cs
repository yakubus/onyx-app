namespace Abstractions.Messaging;

public abstract record EntityBusinessModel
{
    private readonly List<IDomainEvent> _domainEvents;

    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents;

    protected EntityBusinessModel(IEnumerable<IDomainEvent> domainEvents)
    {
        _domainEvents = domainEvents.ToList();
    }
}