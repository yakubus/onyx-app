using Budget.Domain.Accounts;
using Budget.Domain.Counterparties;
using Budget.Domain.Subcategories;
using Budget.Domain.Transactions;

#pragma warning disable CS8846 

namespace Budget.Application.Transactions.GetTransactions;

internal static class GetTransactionFilters
{
    private static string GetAllFilter() => "Id IS NOT NULL";

    private static string GetAccountFilter(AccountId accountId) => $"AccountId = '{accountId}'";

    private static string GetSubcategoryFilter(SubcategoryId subcategoryId) =>
        $"SubcategoryId IS NOT NULL AND SubcategoryId = '{subcategoryId}'";

    private static Task GetCounterpartyFilter(CounterpartyId counterpartyId) =>
        

    internal static Task GetFilter(
        GetTransactionQueryRequest query,
        GetTransactionsQuery request) =>
        query switch
        {
            _ when query == GetTransactionQueryRequest.All ||
                   query == GetTransactionQueryRequest.Empty =>
                GetAllFilter(),
            _ when query == GetTransactionQueryRequest.Account =>
                GetAccountFilter(new (request.AccountId!.Value)),
            _ when query == GetTransactionQueryRequest.Subcategory =>
                GetSubcategoryFilter(new (request.SubcategoryId!.Value)),
            _ when query == GetTransactionQueryRequest.Counterparty =>
                GetCounterpartyFilter(new (request.CounterpartyId!.Value)),
        };
}