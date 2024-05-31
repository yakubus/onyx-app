using SharedDAL.DataModels.Abstractions;

namespace Budget.Infrastructure.Data.DataModels;

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
}