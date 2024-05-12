using Models.Responses;

namespace Budget.Application.Transactions.GetTransactions;

internal static class GetTransactionErrors
{
    internal static readonly Error InvalidQueryValues = new(
        "GetTransaction.InvalidQueryValues",
        "Invalid values for query");
    internal static readonly Error QueryIsNull = new (
        "GetTransaction.QueryIsNull",
        "Pass the query");
}