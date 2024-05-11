using Budget.Application.Shared.Models;

namespace Budget.API.Controllers.Transactions.Requests;

internal sealed record AddTransactionRequest
{
    public Guid AccountId { get; set; }
    public MoneyModel Amount { get; set; }
    public DateTime TransactedAt { get; set; }
    public string CounterpartyName { get; set; }
    public Guid? SubcategoryId { get; set; }
}