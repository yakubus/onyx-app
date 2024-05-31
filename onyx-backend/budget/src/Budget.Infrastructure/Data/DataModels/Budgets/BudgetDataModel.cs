using System.Reflection;
using Budget.Domain.Budgets;
using Models.DataTypes;
using SharedDAL.DataModels.Abstractions;

namespace Budget.Infrastructure.Data.DataModels.Budgets;

internal sealed class BudgetDataModel : IDataModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string BaseCurrency { get; set; }
    public IEnumerable<string> UserIds { get; set; }

    public static BudgetDataModel FromDomainModel(Domain.Budgets.Budget budget)
    {
        return new BudgetDataModel
        {
            Id = budget.Id.Value,
            Name = budget.Name.Value,
            BaseCurrency = budget.BaseCurrency.Code,
            UserIds = budget.UserIds
        };
    }

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
}