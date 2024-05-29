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
        var entities = await Task.Run(
            () => Container.GetItemLinqQueryable<Account>(true)
                .Where(a => a.Name == accountName)
                .AsEnumerable(),
            cancellationToken);

        var entity = entities.SingleOrDefault();

        return entity is null ?
            Result.Failure<Account>(DataAccessErrors<Category>.NotFound) :
            Result.Success(entity);
    }
}