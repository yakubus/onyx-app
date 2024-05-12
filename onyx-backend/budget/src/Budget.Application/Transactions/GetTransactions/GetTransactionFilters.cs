using System.Linq.Expressions;
using Budget.Domain.Transactions;

namespace Budget.Application.Transactions.GetTransactions;

internal static class GetTransactionFilters
{
    private static Expression<Func<Transaction, bool>> GetAllFilter() =>
        _ => true;
    private static Expression<Func<Transaction, bool>> GetAccountFilter(Guid accountId) =>
        transaction => transaction.AccountId.Value == accountId;
    private static Expression<Func<Transaction, bool>> GetSubcategoryFilter(Guid subcategoryId) =>
        transaction => transaction.SubcategoryId != null && transaction.SubcategoryId.Value == subcategoryId;
    private static Expression<Func<Transaction, bool>> GetCounterpartyFilter(Guid counterpartyId) =>
        transaction => transaction.CounterpartyId != null && transaction.CounterpartyId.Value == counterpartyId;

    internal static Expression<Func<Transaction, bool>> GetFilter(
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
                GetCounterpartyFilter(request.CounterpartyId!.Value)
        };
}