using System.Linq.Expressions;
using Models.Responses;

namespace Budget.Domain.Transactions;

public interface ITransactionRepository
{
    Task<Result<IEnumerable<Transaction>>> GetWhereAsync(
        Expression<Func<Transaction, bool>> filterPredicate, 
        CancellationToken cancellationToken = default);

    Task<Result> RemoveRangeAsync(IEnumerable<Transaction> transactions, CancellationToken cancellationToken = default);

    Task<Result<Transaction>> AddAsync(Transaction transaction, CancellationToken cancellationToken = default);

    Task<Result<Transaction>> GetByIdAsync(TransactionId requestTransactionId, CancellationToken cancellationToken = default);

    Task<Result> RemoveAsync(TransactionId transactionId, CancellationToken cancellationToken = default);
}