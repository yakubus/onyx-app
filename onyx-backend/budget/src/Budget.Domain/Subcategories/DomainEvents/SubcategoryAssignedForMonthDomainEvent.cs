using Abstractions.Messaging;
using Models.DataTypes;

namespace Budget.Domain.Subcategories.DomainEvents;

public sealed record SubcategoryAssignedForMonthDomainEvent(
    SubcategoryId SubcategoryId,
    MonthDate AssignedMonth) : IDomainEvent
{
}