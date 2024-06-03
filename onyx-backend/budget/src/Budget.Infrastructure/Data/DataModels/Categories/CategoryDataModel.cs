using System.Reflection;
using Amazon.DynamoDBv2.DocumentModel;
using Budget.Domain.Budgets;
using Budget.Domain.Categories;
using Budget.Domain.Subcategories;
using SharedDAL.DataModels.Abstractions;

namespace Budget.Infrastructure.Data.DataModels.Categories;

internal sealed class CategoryDataModel : IDataModel<Category>
{
    public Guid Id { get; init; }
    public Guid BudgetId { get; init; }
    public string Name { get; init; }
    public IEnumerable<Guid> SubcategoriesId { get; init; }

    private CategoryDataModel(Category category)
    {
        Id = category.Id.Value;
        BudgetId = category.BudgetId.Value;
        Name = category.Name.Value;
        SubcategoriesId = category.SubcategoriesId.Select(sid => sid.Value);
    }

    private CategoryDataModel(Document doc)
    {
        Id = doc[nameof(Id)].AsGuid();
        BudgetId = doc[nameof(BudgetId)].AsGuid();
        Name = doc[nameof(Name)];
        SubcategoriesId = doc[nameof(SubcategoriesId)].AsArrayOfString().Select(Guid.Parse);
    }

    public static CategoryDataModel FromDomainModel(Category category) => new(category);

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

    public static CategoryDataModel FromDocument(Document doc) => new(doc);
}