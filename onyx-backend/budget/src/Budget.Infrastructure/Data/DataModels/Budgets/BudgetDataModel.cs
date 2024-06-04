using System.Reflection;
using Amazon.DynamoDBv2.DocumentModel;
using Budget.Domain.Budgets;
using Models.DataTypes;
using SharedDAL.DataModels;
using SharedDAL.DataModels.Abstractions;

namespace Budget.Infrastructure.Data.DataModels.Budgets;

internal sealed class BudgetDataModel : IDataModel<Domain.Budgets.Budget>
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public string BaseCurrency { get; init; }
    public IEnumerable<string> UserIds { get; init; }

    private BudgetDataModel(Document doc)
    {
        Id = doc[nameof(Id)].AsGuid();
        Name = doc[nameof(Name)];
        BaseCurrency = doc[nameof(BaseCurrency)];
        UserIds = doc[nameof(UserIds)].AsArrayOfString();
    }

    private BudgetDataModel(Domain.Budgets.Budget budget)
    {
        Id = budget.Id.Value;
        Name = budget.Name.Value;
        BaseCurrency = budget.BaseCurrency.Code;
        UserIds = budget.UserIds;
    }

    public static BudgetDataModel FromDomainModel(Domain.Budgets.Budget budget) => new(budget);

    public Type GetDomainModelType() => typeof(Domain.Budgets.Budget);

    public Domain.Budgets.Budget ToDomainModel()
    {
        var id = new BudgetId(Id);

        var name = Activator.CreateInstance(
                       typeof(BudgetName),
                       BindingFlags.Instance | BindingFlags.NonPublic,
                       null,
                       [Name],
                       null) ??
                   throw new DataModelConversionException(
                       typeof(string),
                       typeof(BudgetName),
                       typeof(BudgetDataModel));

        var baseCurrency = Activator.CreateInstance(
                               typeof(Currency),
                               BindingFlags.Instance | BindingFlags.NonPublic,
                               null,
                               [BaseCurrency],
                               null) ??
                           throw new DataModelConversionException(
                               typeof(string),
                               typeof(Currency),
                               typeof(BudgetDataModel));

        return Activator.CreateInstance(
                   typeof(Domain.Budgets.Budget),
                   BindingFlags.Instance | BindingFlags.NonPublic,
                   null,
                   [name, baseCurrency, UserIds.ToList(), id],
                   null) as Domain.Budgets.Budget ??
               throw new DataModelConversionException(
                   typeof(BudgetDataModel),
                   typeof(Domain.Budgets.Budget));
    }

    public static BudgetDataModel FromDocument(Document doc) => new(doc);
}