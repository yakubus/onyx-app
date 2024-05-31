using Budget.Application.Abstractions.Identity;
using Budget.Domain.Accounts;
using Budget.Domain.Categories;
using Models.Responses;
using SharedDAL;
using CosmosDbContext = SharedDAL.CosmosDbContext;

namespace Budget.Infrastructure.Repositories;

internal sealed class AccountRepository : BaseBudgetRepository<Account, AccountId>, IAccountRepository
{
    public AccountRepository(CosmosDbContext context, IBudgetContext budgetContext) : base(context, budgetContext)
    {
    }

    public async Task<Result<Account>> GetByNameAsync(AccountName accountName, CancellationToken cancellationToken)
    {
        return GetFirst(a => a.Name == accountName, cancellationToken);
    }
}