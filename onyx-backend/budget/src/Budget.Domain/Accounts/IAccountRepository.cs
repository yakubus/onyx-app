using Models.Responses;

namespace Budget.Domain.Accounts;

public interface IAccountRepository
{
    Task<Result<Account>> AddAsync(Account account, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<Account>>> GetAsync(CancellationToken cancellationToken = default);
    Task<Result<Account>> UpdateAsync(Account account, CancellationToken cancellationToken = default);
    Task<Result<Account>> DeleteAsync(Account account, CancellationToken cancellationToken = default);
}