using Budget.Domain.Accounts;
using SharedDAL.DataModels.Abstractions;

namespace Budget.Infrastructure.Data.DataModels;

internal sealed class AccountDataModel : IDataModel
{
    public Guid Id { get; set; }
    public Guid BudgetId { get; set; }
    public string Name { get; set; }
    public decimal BalanceAmount { get; set; }
    public string BalanceCurrency { get; set; }
    public string Type { get; set; }

    public Type GetDomainModelType() => typeof(Account);

    public static AccountDataModel FromDomainModel(Account account) =>
        new AccountDataModel
        {
            Id = account.Id.Value,
            BudgetId = account.BudgetId.Value,
            Name = account.Name.Value,
            BalanceAmount = account.Balance.Amount,
            BalanceCurrency = account.Balance.Currency.Code,
            Type = account.Type.Value
        };

}