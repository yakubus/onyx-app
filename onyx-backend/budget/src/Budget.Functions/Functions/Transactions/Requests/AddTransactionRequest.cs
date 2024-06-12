using Budget.Application.Shared.Models;

namespace Budget.Functions.Functions.Transactions.Requests;

public sealed record AddTransactionRequest(
    Guid AccountId,
    MoneyModel Amount,
    DateTime TransactedAt,
    string CounterpartyName,
    Guid? SubcategoryId);