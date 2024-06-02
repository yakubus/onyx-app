using Abstractions.Messaging;
using Budget.Application.Accounts.Models;
using Budget.Domain.Accounts;
using Budget.Domain.Budgets;
using Models.Responses;

namespace Budget.Application.Accounts.AddAccount;

internal sealed class AddAccountCommandHandler : ICommandHandler<AddAccountCommand, AccountModel>
{
    private readonly IAccountRepository _accountRepository;
    private readonly IBudgetRepository _budgetRepository;

    public AddAccountCommandHandler(IAccountRepository accountRepository, IBudgetRepository budgetRepository)
    {
        _accountRepository = accountRepository;
        _budgetRepository = budgetRepository;
    }

    public async Task<Result<AccountModel>> Handle(AddAccountCommand request, CancellationToken cancellationToken)
    {
        var accountsGetResult = await _accountRepository.GetAllAsync(cancellationToken);

        if (accountsGetResult.IsFailure)
        {
            return accountsGetResult.Error;
        }

        var budgetGetResult = await _budgetRepository.GetCurrentBudgetAsync(cancellationToken);

        if (budgetGetResult.IsFailure)
        {
            return budgetGetResult.Error;
        }

        var budget = budgetGetResult.Value;

        var isNewAccountForbidden = accountsGetResult.Value.Count() >= budget.MaxAccounts;

        if (isNewAccountForbidden)
        {
            return AddAccountErrors.AccountMaxCountReached;
        }

        var moneyCreateResult = request.Balance.ToDomainModel();

        if (moneyCreateResult.IsFailure)
        {
            return Result.Failure<AccountModel>(moneyCreateResult.Error);
        }

        var balance = moneyCreateResult.Value;

        var accountCreateResult = Account.Create(request.Name, balance, request.AccountType, new (request.BudgetId));

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
}
