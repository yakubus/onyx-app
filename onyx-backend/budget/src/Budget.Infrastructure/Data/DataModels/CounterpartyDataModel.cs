using Budget.Domain.Counterparties;
using SharedDAL.DataModels.Abstractions;

namespace Budget.Infrastructure.Data.DataModels;

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
}