using System.Reflection;
using Budget.Domain.Budgets;
using Budget.Domain.Categories;
using Budget.Domain.Subcategories;
using Models.Exceptions;
using SharedDAL.DataModels.Abstractions;

namespace Budget.Infrastructure.Data.DataModels.Categories;

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

    public Category ToDomainModel()
    {
        var id = new CategoryId(Id);
        var budgetId = new BudgetId(BudgetId);
        var subcategoriesId = SubcategoriesId.Select(sid => new SubcategoryId(sid)).ToList();

        var name = Activator.CreateInstance(
                       typeof(CategoryName),
                       BindingFlags.Instance | BindingFlags.NonPublic,
                       null,
                       [Name],
                       null) as CategoryName ??
                   throw new DataModelConversionException(
                       typeof(string),
                       typeof(CategoryName),
                       typeof(CategoryDataModel));

        return Activator.CreateInstance(
            typeof(Category),
            BindingFlags.Instance | BindingFlags.NonPublic,
            null,
            [name, subcategoriesId, budgetId, id],
            null) as Category ??
               throw new DataModelConversionException(
                   typeof(CategoryDataModel),
                   typeof(Category));
    }
}