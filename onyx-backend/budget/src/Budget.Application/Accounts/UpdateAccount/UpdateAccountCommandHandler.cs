using Abstractions.Messaging;
using Budget.Application.Accounts.AddAccount;
using Budget.Application.Accounts.Models;
using Budget.Application.Shared.Models;
using Budget.Domain.Accounts;
using Models.Responses;

namespace Budget.Application.Accounts.UpdateAccount;

internal sealed class UpdateAccountCommandHandler : ICommandHandler<UpdateAccountCommand, AccountModel>
{
    private readonly IAccountRepository _accountRepository;

    public UpdateAccountCommandHandler(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public async Task<Result<AccountModel>> Handle(UpdateAccountCommand request, CancellationToken cancellationToken)
    {
        var accountId = new AccountId(request.Id);

        var accountGetResult = await _accountRepository.GetByIdAsync(accountId, cancellationToken);

        if (accountGetResult.IsFailure)
        {
            return Result.Failure<AccountModel>(accountGetResult.Error);
        }

        var account = accountGetResult.Value;

        var updateAccountNameResult = UpdateAccountName(account, request.NewName);

        if (updateAccountNameResult.IsFailure)
        {
            return Result.Failure<AccountModel>(updateAccountNameResult.Error);
        }

        var updateAccountBalanceResult = UpdateAccountBalance(account, request.NewBalance);

        if (updateAccountBalanceResult.IsFailure)
        {
            return Result.Failure<AccountModel>(updateAccountBalanceResult.Error);
        }

        var accountIsNotUniqueResult = await _accountRepository.GetByNameAsync(account.Name, cancellationToken);

        if (accountIsNotUniqueResult.IsSuccess && request.NewName is not null)
        {
            return Result.Failure<AccountModel>(AddAccountErrors.AccountAlreadyExists);
        }

        var accountUpdateResult = await _accountRepository.UpdateAsync(account, cancellationToken);

        if (accountUpdateResult.IsFailure)
        {
            return Result.Failure<AccountModel>(accountUpdateResult.Error);
        }

        account = accountUpdateResult.Value;

        return AccountModel.FromDomainModel(account);
    }

    private Result<Account> UpdateAccountName(Account account, string? newName)
    {
        if (newName is null)
        {
            return Result.Success(account);
        }

        var accountChangeNameResult = account.ChangeName(newName);

        return accountChangeNameResult.IsFailure ?
            Result.Failure<Account>(accountChangeNameResult.Error) :
            Result.Success(account);
    }

    private Result<Account> UpdateAccountBalance(Account account, MoneyModel? newBalance)
    {
        if(newBalance is null)
        {
            return Result.Success(account);
        }

        var newBalanceMoneyCreateResult = newBalance.ToDomainModel();
        
        if(newBalanceMoneyCreateResult.IsFailure)
        {
            return Result.Failure<Account>(newBalanceMoneyCreateResult.Error);
        }

        var newBalanceMoney = newBalanceMoneyCreateResult.Value;

        var accountChangeBalanceResult = account.ChangeBalance(newBalanceMoney);

        return accountChangeBalanceResult.IsFailure ?
            Result.Failure<Account>(accountChangeBalanceResult.Error) :
            Result.Success(account);
    }
}