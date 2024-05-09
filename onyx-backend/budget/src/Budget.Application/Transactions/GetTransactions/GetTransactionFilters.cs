using System.Linq.Expressions;
using Budget.Domain.Transactions;
using Models.DataTypes;

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
        transaction => transaction.CounterpartyId.Value == counterpartyId;
    private static Expression<Func<Transaction, bool>> GetAssignmentFilter(Guid subcategoryId, MonthDate assignmentPeriod) =>
        transaction =>
            transaction.SubcategoryId != null &&
            transaction.SubcategoryId.Value == subcategoryId &&
            assignmentPeriod.ContainsDate(transaction.TransactedAt);

    internal static Expression<Func<Transaction, bool>> GetFilter(
        GetTransactionQueryRequest query,
        GetTransactionsQuery request) =>
        query switch
        {
            _ when query == GetTransactionQueryRequest.All ||
                   query == GetTransactionQueryRequest.Empty =>
                GetTransactionFilters.GetAllFilter(),
            _ when query == GetTransactionQueryRequest.Account =>
                GetTransactionFilters.GetAccountFilter(request.AccountId!.Value),
            _ when query == GetTransactionQueryRequest.Subcategory =>
                GetTransactionFilters.GetSubcategoryFilter(request.SubcategoryId!.Value),
            _ when query == GetTransactionQueryRequest.Counterparty =>
                GetTransactionFilters.GetCounterpartyFilter(request.CounterpartyId!.Value),
            _ when query == GetTransactionQueryRequest.Assignment =>
                GetTransactionFilters.GetAssignmentFilter(request.SubcategoryId!.Value, request.AssignmentPeriod!),
        };
}