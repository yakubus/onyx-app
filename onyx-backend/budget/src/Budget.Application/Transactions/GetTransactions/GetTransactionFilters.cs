#pragma warning disable CS8846 

namespace Budget.Application.Transactions.GetTransactions;

internal static class GetTransactionFilters
{
    private static string GetAllFilter() => "Id IS NOT NULL";

    private static string GetAccountFilter(Guid accountId) => $"AccountId = '{accountId}'";

    private static string GetSubcategoryFilter(Guid subcategoryId) =>
        $"SubcategoryId IS NOT NULL AND SubcategoryId = '{subcategoryId}'";

    private static string GetCounterpartyFilter(Guid counterpartyId) =>
        $"CounterpartyId IS NOT NULL AND CounterpartyId = '{counterpartyId}'";

    internal static string GetFilter(
        GetTransactionQueryRequest query,
        GetTransactionsQuery request) =>
        query switch
        {
            _ when query == GetTransactionQueryRequest.All ||
                   query == GetTransactionQueryRequest.Empty =>
                GetAllFilter(),
            _ when query == GetTransactionQueryRequest.Account =>
                GetAccountFilter(request.AccountId!.Value),
            _ when query == GetTransactionQueryRequest.Subcategory =>
                GetSubcategoryFilter(request.SubcategoryId!.Value),
            _ when query == GetTransactionQueryRequest.Counterparty =>
                GetCounterpartyFilter(request.CounterpartyId!.Value),
        };
}