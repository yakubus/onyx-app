using Budget.Application.Shared.Models;

namespace Budget.API.Controllers.Transactions.Requests;

public sealed record AddTransactionRequest
{
    public Guid AccountId { get; set; }
    public MoneyModel Amount { get; set; }
    public DateTime TransactedAt { get; set; }
    public string CounterpartyName { get; set; }
    public Guid? SubcategoryId { get; set; }

    private AddTransactionRequest(Guid accountId, MoneyModel amount, DateTime transactedAt, string counterpartyName, Guid? subcategoryId)
    {
        AccountId = accountId;
        Amount = amount;
        TransactedAt = transactedAt;
        CounterpartyName = counterpartyName;
        SubcategoryId = subcategoryId;
    }
}