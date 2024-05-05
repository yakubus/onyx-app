using Abstractions.Messaging;

namespace Budget.Application.Categories.RemoveCategory;

public sealed record RemoveCategoryCommand(Guid CategoryId) : ICommand
{
}