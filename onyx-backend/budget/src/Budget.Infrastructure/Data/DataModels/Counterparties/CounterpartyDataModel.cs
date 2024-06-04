using System.Reflection;
using Amazon.DynamoDBv2.DocumentModel;
using Budget.Domain.Budgets;
using Budget.Domain.Counterparties;
using SharedDAL.DataModels;
using SharedDAL.DataModels.Abstractions;

namespace Budget.Infrastructure.Data.DataModels.Counterparties;

internal sealed class CounterpartyDataModel : IDataModel<Counterparty>
{
    public Guid Id { get; init; }
    public Guid BudgetId { get; init; }
    public string Name { get; init; }
    public string Type { get; init; }

    private CounterpartyDataModel(Counterparty counterparty)
    {
        Id = counterparty.Id.Value;
        BudgetId = counterparty.BudgetId.Value;
        Name = counterparty.Name.Value;
        Type = counterparty.Type.Value;
    }

    private CounterpartyDataModel(Document doc)
    {
        Id = doc[nameof(Id)].AsGuid();
        BudgetId = doc[nameof(BudgetId)].AsGuid();
        Name = doc[nameof(Name)];
        Type = doc[nameof(Type)];
    }

    public static CounterpartyDataModel FromDomainModel(Counterparty counterparty) => new(counterparty);

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

    public static CounterpartyDataModel FromDocument(Document doc) => new(doc);
}