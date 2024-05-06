using Abstractions.Messaging;
using Budget.Application.Subcategories.Models;

namespace Budget.Application.Subcategories.AddSubcategory;

public sealed record AddSubcategoryCommand(Guid ParentCategoryId, string Name) : ICommand<SubcategoryModel>
{
}