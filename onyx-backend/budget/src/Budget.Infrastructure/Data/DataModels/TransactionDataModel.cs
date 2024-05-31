using Budget.Domain.Transactions;
using SharedDAL.DataModels.Abstractions;

namespace Budget.Infrastructure.Data.DataModels;

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
}