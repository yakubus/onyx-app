using Budget.Domain.Categories;
using SharedDAL.DataModels.Abstractions;

namespace Budget.Infrastructure.Data.DataModels;

internal sealed class CategoryDataModel : IDataModel
{
    public Guid Id { get; set; }
    public Guid BudgetId { get; set; }
    public string Name { get; set; }
    public IEnumerable<Guid> SubcategoriesId { get; set; }

    public static CategoryDataModel FromDomainModel(Category category) =>
        new()
        {
            Id = category.Id.Value,
            BudgetId = category.BudgetId.Value,
            Name = category.Name.Value,
            SubcategoriesId = category.SubcategoriesId.Select(sid => sid.Value)
        };

    public Type GetDomainModelType() => typeof(Category);
}