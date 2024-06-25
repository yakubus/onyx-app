using Budget.Domain.Accounts;
using Budget.Domain.Counterparties;
using Budget.Domain.Subcategories;
using Models.DataTypes;
using Models.Responses;

namespace Budget.Domain.Transactions;

public interface ITransactionRepository
{
    Task<Result<IEnumerable<Transaction>>> GetManyByIdAsync(
        IEnumerable<TransactionId> ids,
        CancellationToken cancellationToken = default);

    Task<Result<IEnumerable<Transaction>>> GetAllAsync(CancellationToken cancellationToken);

    Task<Result> RemoveRangeAsync(IEnumerable<TransactionId> transactionsId, CancellationToken cancellationToken = default);

    Task<Result<Transaction>> AddAsync(Transaction transaction, CancellationToken cancellationToken = default);

    Task<Result<Transaction>> GetByIdAsync(TransactionId requestTransactionId, CancellationToken cancellationToken = default);

    Task<Result> RemoveAsync(TransactionId transactionId, CancellationToken cancellationToken = default);

    Task<Result> UpdateRangeAsync(IEnumerable<Transaction> transactions, CancellationToken cancellationToken = default);

    Task<Result<IEnumerable<Transaction>>> GetByAccountAsync(AccountId accountId, CancellationToken cancellationToken);

    Task<Result<IEnumerable<Transaction>>> GetByCounterpartyAsync(CounterpartyId counterpartyId, CancellationToken cancellationToken);

    Task<Result<IEnumerable<Transaction>>> GetForAssignmentAsync(SubcategoryId subcategoryId, Assignment assignment, CancellationToken cancellationToken);

    Task<Result<IEnumerable<Transaction>>> GetBySubcategoryAsync(SubcategoryId subcategoryId, CancellationToken cancellationToken);

    Task<Result<IEnumerable<Transaction>>> GetForTargetAsync(SubcategoryId subcategoryId, Target target, CancellationToken cancellationToken);
}