using Abstractions.Messaging;
using Budget.Application.Accounts.Models;
using Budget.Domain.Accounts;
using Budget.Domain.Accounts.CheckingAccounts;
using Budget.Domain.Accounts.SavingAccounts;
using Models.DataTypes;
using Models.Responses;

namespace Budget.Application.Accounts.AddAccount;

internal sealed class AddAccountCommandHandler : ICommandHandler<AddAccountCommand, AccountModel>
{
    private readonly IAccountRepository _accountRepository;

    public AddAccountCommandHandler(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    // TODO: Add max account validation (5 per budget (increased by 1 for each budget member))
    public async Task<Result<AccountModel>> Handle(AddAccountCommand request, CancellationToken cancellationToken)
    {
        var moneyCreateResult = request.Balance.ToDomainModel();

        if (moneyCreateResult.IsFailure)
        {
            return Result.Failure<AccountModel>(moneyCreateResult.Error);
        }

        var balance = moneyCreateResult.Value;

        var accountTypeCreateResult = AccountType.Create(request.AccountType);

        if (accountTypeCreateResult.IsFailure)
        {
            return Result.Failure<AccountModel>(accountTypeCreateResult.Error);
        }

        var accountType = accountTypeCreateResult.Value;

        if (!accountCreateDictionary.TryGetValue(accountType, out var accountCreateFunc))
        {
            return Result.Failure<AccountModel>(AddAccountErrors.NotSupportedAccountType);
        }

        var accountCreateResult = accountCreateFunc.Invoke(request.Name, balance);

        if (accountCreateResult.IsFailure)
        {
            return Result.Failure<AccountModel>(accountCreateResult.Error);
        }

        var account = accountCreateResult.Value;

        var accountIsNotUniqueResult = await _accountRepository.GetByNameAsync(account.Name, cancellationToken);

        if (accountIsNotUniqueResult.IsSuccess)
        {
            return Result.Failure<AccountModel>(AddAccountErrors.AccountAlreadyExists);
        }

        var addAccountResult = await _accountRepository.AddAsync(account, cancellationToken);

        if (addAccountResult.IsFailure)
        {
            return Result.Failure<AccountModel>(addAccountResult.Error);
        }

        var createdAccount = addAccountResult.Value;
        var createdAccountModel = AccountModel.FromDomainModel(createdAccount);

        return createdAccountModel;
    }

    private static readonly IReadOnlyDictionary<AccountType, Func<string, Money, Result<Account>>> accountCreateDictionary = new
        Dictionary<AccountType, Func<string, Money, Result<Account>>>
    {
        {AccountType.Checking, CheckingAccount.Create},
        {AccountType.Savings, SavingsAccount.Create}
    };
}
