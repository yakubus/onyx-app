using Budget.Domain.Subcategories;
using Models.Responses;

namespace Budget.Domain.Accounts;

public interface IAccountRepository
{
    Task<Result<Account>> AddAsync(Account account, CancellationToken cancellationToken = default);
    Task<Result<Account>> UpdateAsync(Account account, CancellationToken cancellationToken = default);
    Task<Result> RemoveAsync(AccountId accountId, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<Account>>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Result<Account>> GetByIdAsync(AccountId accountId, CancellationToken cancellationToken);
    Task<Result<Account>> GetByNameAsync(AccountName accountName, CancellationToken cancellationToken);
}