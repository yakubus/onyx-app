using Abstractions.Messaging;

namespace Abstractions.DomainBaseTypes;

public interface IEntity
{
    IReadOnlyList<IDomainEvent> GetDomainEvents();
    void ClearDomainEvents();
}