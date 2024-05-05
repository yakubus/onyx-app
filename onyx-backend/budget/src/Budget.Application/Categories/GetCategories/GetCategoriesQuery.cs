using Abstractions.Messaging;
using Budget.Application.Categories.Models;

namespace Budget.Application.Categories.GetCategories;

public sealed record GetCategoriesQuery : IQuery<IEnumerable<CategoryModel>>, ICommand<IEnumerable<CategoryModel>>
{
}