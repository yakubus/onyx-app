using Abstractions.Messaging;

namespace Budget.Application.Subcategories.RemoveSubcategory;

public sealed record RemoveSubcategoryCommand(Guid Id) : ICommand
{
}