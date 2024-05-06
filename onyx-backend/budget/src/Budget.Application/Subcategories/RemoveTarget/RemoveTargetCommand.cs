using Abstractions.Messaging;

namespace Budget.Application.Subcategories.RemoveTarget;

public sealed record RemoveTargetCommand(Guid SubcategoryId) : ICommand
{
}