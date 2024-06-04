using System.Reflection;
using Amazon.DynamoDBv2.DocumentModel;
using Budget.Domain.Accounts;
using Budget.Domain.Budgets;
using Budget.Domain.Counterparties;
using Budget.Domain.Subcategories;
using Budget.Domain.Transactions;
using Models.DataTypes;
using SharedDAL.DataModels;
using SharedDAL.DataModels.Abstractions;

namespace Budget.Infrastructure.Data.DataModels.Transactions;

internal sealed class TransactionDataModel : IDataModel<Transaction>
{
    public Guid Id { get; init; }
    public Guid BudgetId { get; init; }
    public Guid AccountId { get; init; }
    public Guid? SubcategoryId { get; init; }
    public Guid? CounterpartyId { get; init; }
    public int TransactedAtDay { get; init; }
    public int TransactedAtMonth { get; init; }
    public int TransactedAtYear { get; init; }
    public int TransactedAtHour { get; init; }
    public int TransactedAtMinute { get; init; }
    public int TransactedAtSecond { get; init; }
    public decimal AmountAmount { get; init; }
    public string AmountCurrency { get; init; }
    public decimal BudgetAmountAmount { get; init; }
    public string BudgetAmountCurrency { get; init; }
    public decimal OriginalAmountAmount { get; init; }
    public string OriginalAmountCurrency { get; init; }

    public TransactionDataModel(Transaction transaction)
    {
        var transactedAt = transaction.TransactedAt.ToUniversalTime();
        Id = transaction.Id.Value;
        BudgetId = transaction.BudgetId.Value;
        AccountId = transaction.AccountId.Value;
        SubcategoryId = transaction.SubcategoryId?.Value;
        CounterpartyId = transaction.CounterpartyId?.Value;
        TransactedAtDay = transactedAt.Day;
        TransactedAtMonth = transactedAt.Month;
        TransactedAtYear = transactedAt.Year;
        TransactedAtHour = transactedAt.Hour;
        TransactedAtMinute = transactedAt.Minute;
        TransactedAtSecond = transactedAt.Second;
        AmountAmount = transaction.Amount.Amount;
        AmountCurrency = transaction.Amount.Currency.Code;
        BudgetAmountAmount = transaction.BudgetAmount.Amount;
        BudgetAmountCurrency = transaction.BudgetAmount.Currency.Code;
        OriginalAmountAmount = transaction.OriginalAmount.Amount;
        OriginalAmountCurrency = transaction.OriginalAmount.Currency.Code;
    }

    public TransactionDataModel(Document doc)
    {
        Id = doc[nameof(Id)].AsGuid();
        BudgetId = doc[nameof(BudgetId)].AsGuid();
        AccountId = doc[nameof(AccountId)].AsGuid();
        SubcategoryId = doc[nameof(SubcategoryId)].AsGuid();
        CounterpartyId = doc[nameof(CounterpartyId)].AsGuid();
        TransactedAtDay = doc[nameof(TransactedAtDay)].AsInt();
        TransactedAtMonth = doc[nameof(TransactedAtMonth)].AsInt();
        TransactedAtYear = doc[nameof(TransactedAtYear)].AsInt();
        TransactedAtHour = doc[nameof(TransactedAtHour)].AsInt();
        TransactedAtMinute = doc[nameof(TransactedAtMinute)].AsInt();
        TransactedAtSecond = doc[nameof(TransactedAtSecond)].AsInt();
        AmountAmount = doc[nameof(AmountAmount)].AsDecimal();
        AmountCurrency = doc[nameof(AmountCurrency)];
        BudgetAmountAmount = doc[nameof(BudgetAmountAmount)].AsDecimal();
        BudgetAmountCurrency = doc[nameof(BudgetAmountCurrency)];
        OriginalAmountAmount = doc[nameof(OriginalAmountAmount)].AsDecimal();
        OriginalAmountCurrency = doc[nameof(OriginalAmountCurrency)];
    }

    public static TransactionDataModel FromDomainModel(Transaction transaction) => new(transaction);

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

        var transactedAt = new DateTime(
            TransactedAtYear,
            TransactedAtMonth,
            TransactedAtDay,
            TransactedAtHour,
            TransactedAtMinute,
            TransactedAtSecond,
            DateTimeKind.Utc);

        return Activator.CreateInstance(
                   typeof(Transaction),
                   BindingFlags.Instance | BindingFlags.NonPublic,
                   null,
                   [
                       accountId,
                       amount,
                       originalAmount,
                       transactedAt,
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

    public static TransactionDataModel FromDocument(Document doc) => new(doc);
}