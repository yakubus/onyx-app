using Abstractions.Messaging;
using Budget.Application.Accounts.Models;
using Budget.Domain.Accounts;
using Budget.Domain.Subcategories;
using Budget.Domain.Transactions;
using Models.Responses;

namespace Budget.Application.Transactions.RemoveTransaction;

internal sealed class RemoveTransactionCommandHandler : ICommandHandler<RemoveTransactionCommand>
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IAccountRepository _accountRepository;
    private readonly ISubcategoryRepository _subcategoryRepository;

    public RemoveTransactionCommandHandler(ITransactionRepository transactionRepository, ISubcategoryRepository subcategoryRepository, IAccountRepository accountRepository)
    {
        _transactionRepository = transactionRepository;
        _subcategoryRepository = subcategoryRepository;
        _accountRepository = accountRepository;
    }

    public async Task<Result> Handle(RemoveTransactionCommand request, CancellationToken cancellationToken)
    {
        var requestTransactionId = new TransactionId(request.TransactionId);
        var getTransactionResult = await _transactionRepository.GetByIdAsync(requestTransactionId, cancellationToken);

        if (getTransactionResult.IsFailure)
        {
            return Result.Failure(getTransactionResult.Error);
        }

        var transaction = getTransactionResult.Value;

        var subcategoryGetResult = transaction.SubcategoryId is null ? 
            null :
            await _subcategoryRepository.GetByIdAsync(transaction.SubcategoryId, cancellationToken);

        if (subcategoryGetResult is { IsFailure: true })
        {
            return Result.Failure(subcategoryGetResult.Error);
        }

        var accountGetResult = await _accountRepository.GetByIdAsync(transaction.AccountId, cancellationToken);

        if (accountGetResult.IsFailure)
        {
            return Result.Failure(accountGetResult.Error);
        }

        var subcategory = subcategoryGetResult?.Value;
        var account = accountGetResult.Value;

        TransactionService.RemoveTransaction(transaction, account, subcategory);

        var transactionRemoveResult = await _transactionRepository.RemoveAsync(transaction.Id, cancellationToken);

        if(transactionRemoveResult.IsFailure)
        {
            return Result.Failure(transactionRemoveResult.Error);
        }

        var accountUpdateResult = await _accountRepository.UpdateAsync(account, cancellationToken);

        if (accountUpdateResult.IsFailure)
        {
            return Result.Failure(accountUpdateResult.Error);
        }

        if (subcategory is null)
        {
            return Result.Success();
        }

        var subcategoryUpdateResult = await _subcategoryRepository.UpdateAsync(subcategory, cancellationToken);

        if (subcategoryUpdateResult.IsFailure)
        {
            return Result.Failure(subcategoryUpdateResult.Error);
        }

        return Result.Success();
    }
}