using Abstractions.Messaging;
using Budget.Application.Accounts.Models;
using Budget.Domain.Accounts;
using Models.Responses;

namespace Budget.Application.Accounts.GetAccounts;

internal sealed class GetAccountsQueryHandler : IQueryHandler<GetAccountsQuery, IEnumerable<AccountModel>>
{
    private readonly IAccountRepository _accountRepository;

    public GetAccountsQueryHandler(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public async Task<Result<IEnumerable<AccountModel>>> Handle(GetAccountsQuery request, CancellationToken cancellationToken)
    {
        var accountsGetResult = await _accountRepository.GetAllAsync(cancellationToken);

        if (accountsGetResult.IsFailure)
        {
            return Result.Failure<IEnumerable<AccountModel>>(accountsGetResult.Error);
        }

        var accounts = accountsGetResult.Value;

        return Result.Create(accounts.Select(AccountModel.FromDomainModel));
    }
}