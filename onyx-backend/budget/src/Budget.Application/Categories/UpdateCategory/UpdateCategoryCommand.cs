using Abstractions.Messaging;
using Budget.Application.Categories.Models;

namespace Budget.Application.Categories.UpdateCategory;

public sealed record UpdateCategoryCommand(Guid Id, string NewName) : ICommand<CategoryModel>
{
}