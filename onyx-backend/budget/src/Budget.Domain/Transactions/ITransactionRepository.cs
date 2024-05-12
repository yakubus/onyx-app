using System.Linq.Expressions;
using Abstractions.DomainBaseTypes;
using Budget.Domain.Subcategories;
using Models.Responses;

namespace Budget.Domain.Transactions;

public interface ITransactionRepository
{
    Task<Result<IEnumerable<Transaction>>> GetWhereAsync(
        Expression<Func<Transaction, bool>> filterPredicate, 
        CancellationToken cancellationToken = default);

    Task<Result<IEnumerable<Transaction>>> GetWhereAsync(
        string sqlQuery,
        KeyValuePair<string, object>? parameter,
        CancellationToken cancellationToken);

    Task<Result<IEnumerable<Transaction>>> GetManyByIdAsync(
        IEnumerable<TransactionId> ids,
        CancellationToken cancellationToken = default);

    Task<Result> RemoveRangeAsync(IEnumerable<TransactionId> transactionsId, CancellationToken cancellationToken = default);

    Task<Result<Transaction>> AddAsync(Transaction transaction, CancellationToken cancellationToken = default);

    Task<Result<Transaction>> GetByIdAsync(TransactionId requestTransactionId, CancellationToken cancellationToken = default);

    Task<Result> RemoveAsync(TransactionId transactionId, CancellationToken cancellationToken = default);
}