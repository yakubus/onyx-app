using Abstractions.Messaging;

namespace Budget.Domain.Subcategories.DomainEvents;

public sealed record TargetMoveDomainEvent(SubcategoryId SubcategoryId) : IDomainEvent
{
}