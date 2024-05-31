using Budget.Application.Abstractions.Messaging;
using Budget.Application.Categories.Models;

namespace Budget.Application.Categories.AddCategory;

public sealed record AddCategoryCommand(string Name, Guid BudgetId) : BudgetCommand<CategoryModel>(BudgetId)
{
}