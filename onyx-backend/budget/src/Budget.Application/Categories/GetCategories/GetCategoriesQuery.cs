using Budget.Application.Abstractions.Messaging;
using Budget.Application.Categories.Models;

namespace Budget.Application.Categories.GetCategories;

public sealed record GetCategoriesQuery(Guid BudgetId) : BudgetQuery<IEnumerable<CategoryModel>>(BudgetId);