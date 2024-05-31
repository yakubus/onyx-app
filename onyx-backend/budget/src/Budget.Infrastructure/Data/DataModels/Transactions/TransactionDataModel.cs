using System.Reflection;
using Budget.Domain.Accounts;
using Budget.Domain.Budgets;
using Budget.Domain.Counterparties;
using Budget.Domain.Subcategories;
using Budget.Domain.Transactions;
using Models.DataTypes;
using SharedDAL.DataModels.Abstractions;

namespace Budget.Infrastructure.Data.DataModels.Transactions;

internal sealed class TransactionDataModel : IDataModel
{
    public Guid Id { get; set; }
    public Guid BudgetId { get; set; }
    public Guid AccountId { get; set; }
    public Guid? SubcategoryId { get; set; }
    public Guid? CounterpartyId { get; set; }
    //[JsonConverter(typeof(DateTimeConverter))]
    public DateTime TransactedAt { get; set; }
    public decimal AmountAmount { get; set; }
    public string AmountCurrency { get; set; }
    public decimal BudgetAmountAmount { get; set; }
    public string BudgetAmountCurrency { get; set; }
    public decimal OriginalAmountAmount { get; set; }
    public string OriginalAmountCurrency { get; set; }

    public static TransactionDataModel FromDomainModel(Transaction transaction)
    {
        return new TransactionDataModel
        {
            Id = transaction.Id.Value,
            BudgetId = transaction.BudgetId.Value,
            AccountId = transaction.AccountId.Value,
            SubcategoryId = transaction.SubcategoryId?.Value,
            CounterpartyId = transaction.CounterpartyId?.Value,
            TransactedAt = transaction.TransactedAt,
            AmountAmount = transaction.Amount.Amount,
            AmountCurrency = transaction.Amount.Currency.Code,
            BudgetAmountAmount = transaction.BudgetAmount.Amount,
            BudgetAmountCurrency = transaction.BudgetAmount.Currency.Code,
            OriginalAmountAmount = transaction.OriginalAmount.Amount,
            OriginalAmountCurrency = transaction.OriginalAmount.Currency.Code
        };
    }

    public Type GetDomainModelType() => typeof(Transaction);

    public Transaction ToDomainModel()
    {
        var id = new TransactionId(Id);
        var budgetId = new BudgetId(BudgetId);
        var accountId = new AccountId(AccountId);
        var subcategoryId = SubcategoryId switch
        {
            not null => new SubcategoryId(SubcategoryId.Value), 
            _ => null
        };
        var counterpartyId = CounterpartyId switch
        {
            not null => new CounterpartyId(CounterpartyId.Value), 
            _ => null
        };

        var (amountCurrency, budgetAmountCurrency, originalAmountCurrency) =
            (Activator.CreateInstance(
                 typeof(Currency),
                 BindingFlags.Instance | BindingFlags.NonPublic,
                 null,
                 [AmountCurrency],
                 null) as Currency ??
             throw new DataModelConversionException(
                 AmountCurrency,
                 typeof(Currency),
                 this),
                Activator.CreateInstance(
                    typeof(Currency),
                    BindingFlags.Instance | BindingFlags.NonPublic,
                    null,
                    [BudgetAmountCurrency],
                    null) as Currency ??
                throw new DataModelConversionException(
                    BudgetAmountCurrency,
                    typeof(Currency),
                    this),
                Activator.CreateInstance(
                    typeof(Currency),
                    BindingFlags.Instance | BindingFlags.NonPublic,
                    null,
                    [OriginalAmountCurrency],
                    null) as Currency ??
                throw new DataModelConversionException(
                    OriginalAmountCurrency,
                    typeof(Currency),
                    this));

        var (amount, budgetAmount, originalAmount) = (
            new Money(AmountAmount, amountCurrency),
            new Money(BudgetAmountAmount, budgetAmountCurrency),
            new Money(OriginalAmountAmount, originalAmountCurrency));

        return Activator.CreateInstance(
                   typeof(Transaction),
                   BindingFlags.Instance | BindingFlags.NonPublic,
                   null,
                   [
                       accountId,
                       amount,
                       originalAmount,
                       TransactedAt,
                       subcategoryId,
                       counterpartyId,
                       budgetAmount,
                       budgetId,
                       id
                   ],
                   null) as Transaction ??
               throw new DataModelConversionException(
                   this,
                   typeof(Transaction));
    }
}