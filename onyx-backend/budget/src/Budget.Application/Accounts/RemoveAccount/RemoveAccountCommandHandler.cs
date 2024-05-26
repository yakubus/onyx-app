using Abstractions.Messaging;
using Budget.Domain.Accounts;
using Budget.Domain.Subcategories;
using Budget.Domain.Transactions;
using Models.Responses;

namespace Budget.Application.Accounts.RemoveAccount;

internal sealed class RemoveAccountCommandHandler : ICommandHandler<RemoveAccountCommand>
{
    private readonly IAccountRepository _accountRepository;
    private readonly ITransactionRepository _transactionRepository;
    private readonly ISubcategoryRepository _subcategoryRepository;

    public RemoveAccountCommandHandler(
        IAccountRepository accountRepository,
        ITransactionRepository transactionRepository,
        ISubcategoryRepository subcategoryRepository)
    {
        _accountRepository = accountRepository;
        _transactionRepository = transactionRepository;
        _subcategoryRepository = subcategoryRepository;
    }

    public async Task<Result> Handle(RemoveAccountCommand request, CancellationToken cancellationToken)
    {
        var accountId = new AccountId(request.Id);

        var relatedTransactionsGetResult = _transactionRepository.GetWhere(
            transaction => transaction.AccountId == accountId, 
            cancellationToken);

        if (relatedTransactionsGetResult.IsFailure)
        {
            return Result.Failure(relatedTransactionsGetResult.Error);
        }

        var relatedTransactions = relatedTransactionsGetResult.Value.ToList();
        
        if (relatedTransactions.Count == 0)
        {
            return await RemoveAccounts(cancellationToken, accountId);
        }

        var relatedSubcategoriesGetResult = await _subcategoryRepository.GetManyByIdAsync(
            relatedTransactions.Where(t => t.SubcategoryId is not null).Select(t => t.SubcategoryId),
            cancellationToken);

        if (relatedSubcategoriesGetResult.IsFailure)
        {
            return Result.Failure(relatedSubcategoriesGetResult.Error);
        }

        var relatedSubcategories = relatedSubcategoriesGetResult.Value.ToList();

        relatedSubcategories.ForEach(subcategory => relatedTransactions.ToList()
            .ForEach(transaction => subcategory.RemoveTransaction(transaction)));

        return await RemoveAccountSafely(
            relatedTransactions, 
            relatedSubcategories, 
            accountId, 
            cancellationToken);
    }

    private async Task<Result> RemoveAccounts(CancellationToken cancellationToken, AccountId accountId)
    {
        var removeResult = await _accountRepository.RemoveAsync(accountId, cancellationToken);

        return removeResult.IsFailure ? Result.Failure(removeResult.Error) : Result.Success();
    }

    private async Task<Result> RemoveAccountSafely(
        IEnumerable<Transaction> relatedTransactions,
        IEnumerable<Subcategory> relatedSubcategories,
        AccountId accountId,
        CancellationToken cancellationToken)
    { 
        var relatedTransactionsRemoveTask = _transactionRepository.RemoveRangeAsync(
            relatedTransactions.Select(t => t.Id),
            cancellationToken);
        var relatedSubcategoriesUpdateTask = _subcategoryRepository.UpdateRangeAsync(
            relatedSubcategories,
            cancellationToken);
        var accountRemoveTask = _accountRepository.RemoveAsync(accountId, cancellationToken);

        var results = await Task.WhenAll(
            relatedTransactionsRemoveTask,
            relatedSubcategoriesUpdateTask,
            accountRemoveTask);

        var failedResult = results.FirstOrDefault(result => result.IsFailure);

        return failedResult ?? Result.Success();
    }
}