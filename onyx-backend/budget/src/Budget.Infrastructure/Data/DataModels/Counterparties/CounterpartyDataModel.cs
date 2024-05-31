using System.Reflection;
using Budget.Domain.Budgets;
using Budget.Domain.Counterparties;
using SharedDAL.DataModels.Abstractions;

namespace Budget.Infrastructure.Data.DataModels.Counterparties;

internal sealed class CounterpartyDataModel : IDataModel
{
    public Guid Id { get; set; }
    public Guid BudgetId { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }

    public static CounterpartyDataModel FromDomainModel(Counterparty counterparty) =>
        new()
        {
            Id = counterparty.Id.Value,
            BudgetId = counterparty.BudgetId.Value,
            Name = counterparty.Name.Value,
            Type = counterparty.Type.Value,
        };

    public Type GetDomainModelType() => typeof(Counterparty);

    public Counterparty ToDomainModel()
    {
        var id = new CounterpartyId(Id);
        var budgetId = new BudgetId(BudgetId);

        var name = Activator.CreateInstance(
                       typeof(CounterpartyName),
                       BindingFlags.Instance | BindingFlags.NonPublic,
                       null,
                       [Name],
                       null) as CounterpartyName ??
                   throw new DataModelConversionException(
                       Name,
                       typeof(CounterpartyName),
                       this);

        var type = Activator.CreateInstance(
                       typeof(CounterpartyType),
                       BindingFlags.Instance | BindingFlags.NonPublic,
                       null,
                       [Type],
                       null) as CounterpartyType ??
                   throw new DataModelConversionException(
                       Type,
                       typeof(CounterpartyType),
                       this);

        return Activator.CreateInstance(
                   typeof(Counterparty),
                   BindingFlags.Instance | BindingFlags.NonPublic,
                   null,
                   [name, type, budgetId, id],
                   null) as Counterparty ??
               throw new DataModelConversionException(
                   this,
                   typeof(Counterparty));
    }
}