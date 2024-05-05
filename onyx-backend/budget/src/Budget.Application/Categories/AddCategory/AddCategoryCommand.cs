using Abstractions.Messaging;
using Budget.Application.Categories.Models;

namespace Budget.Application.Categories.AddCategory;

public sealed record AddCategoryCommand(string Name) : ICommand<CategoryModel>
{
}