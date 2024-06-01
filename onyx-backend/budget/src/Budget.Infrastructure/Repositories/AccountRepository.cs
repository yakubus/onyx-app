using Budget.Application.Abstractions.Identity;
using Budget.Domain.Accounts;
using Models.Responses;
using SharedDAL;

namespace Budget.Infrastructure.Repositories;

internal sealed class AccountRepository : BaseBudgetRepository<Account, AccountId>, IAccountRepository
{
    public AccountRepository(DbContext context, IBudgetContext budgetContext) : base(context, budgetContext)
    {
    }

    public async Task<Result<Account>> GetByNameAsync(AccountName accountName, CancellationToken cancellationToken)
    {
        return GetFirst(a => a.Name == accountName, cancellationToken);
    }
}