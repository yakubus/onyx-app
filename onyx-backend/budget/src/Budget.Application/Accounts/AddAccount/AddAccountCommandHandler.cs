using Abstractions.Messaging;
using Budget.Application.Accounts.Models;
using Budget.Domain.Accounts;
using Models.Responses;

namespace Budget.Application.Accounts.AddAccount;

internal sealed class AddAccountCommandHandler : ICommandHandler<AddAccountCommand, AccountModel>
{
    private readonly IAccountRepository _accountRepository;

    public AddAccountCommandHandler(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    // TODO: Add max account validation (5 per budget (increased by 2 for each budget member))
    public async Task<Result<AccountModel>> Handle(AddAccountCommand request, CancellationToken cancellationToken)
    {
        var moneyCreateResult = request.Balance.ToDomainModel();

        if (moneyCreateResult.IsFailure)
        {
            return Result.Failure<AccountModel>(moneyCreateResult.Error);
        }

        var balance = moneyCreateResult.Value;

        var accountCreateResult = Account.Create(request.Name, balance, request.AccountType);

        if (accountCreateResult.IsFailure)
        {
            return Result.Failure<AccountModel>(accountCreateResult.Error);
        }

        var account = accountCreateResult.Value;

        var accountIsNotUniqueResult = await _accountRepository.GetByNameAsync(account.Name, cancellationToken);

        if (accountIsNotUniqueResult.IsSuccess)
        {
            return Result.Failure<AccountModel>(AddCounterpartyErrors.AccountAlreadyExists);
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
}
