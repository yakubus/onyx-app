using System.Linq.Expressions;
using Models.Responses;

namespace Budget.Domain.Transactions;

public interface ITransactionRepository
{
    Task<Result<IEnumerable<Transaction>>> GetAllTransactionsAsync(CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<Transaction>>> GetFilteredTransactionsAsync(
        Expression<Func<Transaction, bool>> filterPredicate, 
        CancellationToken cancellationToken = default);
    Task<Result> DeleteRangeAsync(IEnumerable<Transaction> relatedTransactions, CancellationToken cancellationToken);
}