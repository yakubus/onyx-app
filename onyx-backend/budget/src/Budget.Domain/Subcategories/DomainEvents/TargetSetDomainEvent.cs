using Abstractions.Messaging;

namespace Budget.Domain.Subcategories.DomainEvents;

public sealed record TargetSetDomainEvent(SubcategoryId SubcategoryId) : IDomainEvent
{
}