using Budget.Application.Abstractions.Identity;
using Budget.Domain.Accounts;
using Models.Responses;
using SharedDAL;
using SharedDAL.DataModels.Abstractions;

namespace Budget.Infrastructure.Repositories;

internal sealed class AccountRepository : BaseBudgetRepository<Account, AccountId>, IAccountRepository
{
    public AccountRepository(
        DbContext context,
        IBudgetContext budgetContext,
        IDataModelService<Account> dataModelService) : base(
        context,
        budgetContext,
        dataModelService)
    {
    }

    public async Task<Result<Account>> GetByNameAsync(AccountName accountName, CancellationToken cancellationToken)
    {
        return await GetFirst($"Name = {accountName.Value}", cancellationToken);
    }
}