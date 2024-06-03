using Amazon.DynamoDBv2.DocumentModel;
using Budget.Domain.Accounts;
using Budget.Domain.Budgets;
using Models.DataTypes;
using SharedDAL.DataModels.Abstractions;
using BindingFlags = System.Reflection.BindingFlags;

namespace Budget.Infrastructure.Data.DataModels.Accounts;

internal sealed class AccountDataModel : IDataModel<Account>
{
    public Guid Id { get; init; }
    public Guid BudgetId { get; init; }
    public string Name { get; init; }
    public decimal BalanceAmount { get; init; }
    public string BalanceCurrency { get; init; }
    public string Type { get; init; }

    private AccountDataModel(
        Document doc)
    {
        Id = doc[nameof(Id)].AsGuid();
        BudgetId = doc[nameof(BudgetId)].AsGuid();
        Name = doc[nameof(Name)];
        BalanceAmount = doc[nameof(BalanceAmount)].AsDecimal();
        BalanceCurrency = doc[nameof(BalanceCurrency)];
        Type = doc[nameof(Type)];
    }
    private AccountDataModel(
        Account account)
    {
        Id = account.Id.Value;
        BudgetId = account.BudgetId.Value;
        Name = account.Name.Value;
        BalanceAmount = account.Balance.Amount;
        BalanceCurrency = account.Balance.Currency.Code;
        Type = account.Type.Value;
    }

    public Type GetDomainModelType() => typeof(Account);

    public static AccountDataModel FromDomainModel(Account account) => new(account);

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

    public static AccountDataModel FromDocument(Document doc) => new(doc);
}