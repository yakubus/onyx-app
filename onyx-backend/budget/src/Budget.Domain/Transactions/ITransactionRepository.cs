using System.Linq.Expressions;
using Models.Responses;

namespace Budget.Domain.Transactions;

public interface ITransactionRepository
{
    Task<Result<IEnumerable<Transaction>>> GetAllTransactionsAsync(CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<Transaction>>> GetWhereAsync(
        Expression<Func<Transaction, bool>> filterPredicate, 
        CancellationToken cancellationToken = default);
    Task<Result> RemoveRangeAsync(IEnumerable<Transaction> transactions, CancellationToken cancellationToken);
    Task<Result> UpdateRangeAsync(IEnumerable<Transaction> transactions, CancellationToken cancellationToken);
}