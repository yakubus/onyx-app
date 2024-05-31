using Budget.Domain.Accounts;
using Budget.Domain.Budgets;
using Models.DataTypes;
using SharedDAL.DataModels.Abstractions;
using BindingFlags = System.Reflection.BindingFlags;

namespace Budget.Infrastructure.Data.DataModels.Accounts;

internal sealed class AccountDataModel : IDataModel<Account>
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

    public Account ToDomainModel()
    {
        var name = Activator.CreateInstance(
                       typeof(AccountName),
                       BindingFlags.Instance | BindingFlags.NonPublic,
                       null,
                       [Name],
                       null) as AccountName ??
                   throw new DataModelConversionException(
                       typeof(string),
                       typeof(AccountName),
                       typeof(AccountDataModel));

        var balanceCurrency = Activator.CreateInstance(
                                  typeof(Currency),
                                  BindingFlags.Instance | BindingFlags.NonPublic,
                                  null,
                                  [BalanceCurrency],
                                  null) as Currency ??
                              throw new DataModelConversionException(
                                  typeof(string),
                                  typeof(Currency),
                                  typeof(AccountDataModel));

        var balance = new Money(BalanceAmount, balanceCurrency);

        var accountType = Activator.CreateInstance(
                              typeof(AccountType),
                              BindingFlags.Instance | BindingFlags.NonPublic,
                              null,
                              [Type],
                              null) as AccountType ??
                          throw new DataModelConversionException(
                              typeof(string),
                              typeof(AccountType),
                              typeof(AccountDataModel));

        var accountId = new AccountId(Id);
        var budgetId = new BudgetId(BudgetId);

        return Activator.CreateInstance(
                   typeof(Account),
                   BindingFlags.Instance | BindingFlags.NonPublic,
                   null,
                   [name, balance, accountType, budgetId, accountId],
                   null) as Account ??
               throw new DataModelConversionException(
                   typeof(AccountDataModel),
                   typeof(Account));

    }
}