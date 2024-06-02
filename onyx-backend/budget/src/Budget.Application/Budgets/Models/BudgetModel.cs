namespace Budget.Application.Budgets.Models;

public sealed record BudgetModel
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public string Currency { get; init; }
    public IEnumerable<string> UserIds { get; init; }

    private BudgetModel(Guid id, string name, string currency, IEnumerable<string> userIds)
    {
        Id = id;
        Name = name;
        Currency = currency;
        UserIds = userIds;
    }

    internal static BudgetModel FromDomainModel(Domain.Budgets.Budget domainModel) =>
        new (
            domainModel.Id.Value,
            domainModel.Name.Value,
            domainModel.BaseCurrency.Code,
            domainModel.UserIds);
}