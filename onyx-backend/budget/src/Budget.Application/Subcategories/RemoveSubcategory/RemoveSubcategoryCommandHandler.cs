using Abstractions.Messaging;
using Budget.Domain.Categories;
using Budget.Domain.Subcategories;
using Budget.Domain.Transactions;
using Models.Responses;

namespace Budget.Application.Subcategories.RemoveSubcategory;

internal sealed class RemoveSubcategoryCommandHandler : ICommandHandler<RemoveSubcategoryCommand>
{
    private readonly ISubcategoryRepository _subcategoryRepository;
    private readonly ITransactionRepository _transactionRepository;
    private readonly ICategoryRepository _categoryRepository;

    public RemoveSubcategoryCommandHandler(ISubcategoryRepository subcategoryRepository, ITransactionRepository transactionRepository, ICategoryRepository categoryRepository)
    {
        _subcategoryRepository = subcategoryRepository;
        _transactionRepository = transactionRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task<Result> Handle(RemoveSubcategoryCommand request, CancellationToken cancellationToken)
    {
        var subcategoryId = new SubcategoryId(request.Id);

        var subcategoryGetResult = await _subcategoryRepository.GetByIdAsync(subcategoryId, cancellationToken);

        if (subcategoryGetResult.IsFailure)
        {
            return Result.Failure(subcategoryGetResult.Error);
        }

        var subcategory = subcategoryGetResult.Value;

        var categoryGetResult = await _categoryRepository.GetSingleAsync(
            category => category.Subcategories.Any(s => s.Id == subcategory.Id), 
            cancellationToken);

        if (categoryGetResult.IsFailure)
        {
            return Result.Failure(categoryGetResult.Error);
        }

        var category = categoryGetResult.Value;

        var categoryRemoveSubcategoryResult = category.RemoveSubcategory(subcategory);

        if (categoryRemoveSubcategoryResult.IsFailure)
        {
            return Result.Failure(categoryRemoveSubcategoryResult.Error);
        }

        var getTransactionsResult = await _transactionRepository.GetWhereAsync(
            transaction => transaction.Subcategory != null && transaction.Subcategory.Id == subcategory.Id, 
            cancellationToken);

        if (getTransactionsResult.IsFailure)
        {
            return Result.Failure(getTransactionsResult.Error);
        }

        var transactions = getTransactionsResult.Value;

        return await RemoveSubcategorySafely(cancellationToken, category, subcategory, transactions);
    }

    private async Task<Result> RemoveSubcategorySafely(
        CancellationToken cancellationToken,
        Category category,
        Subcategory subcategory,
        IEnumerable<Transaction> transactions)
    {
        var categoryUpdateResult = await _categoryRepository.UpdateAsync(category, cancellationToken);

        if (categoryUpdateResult.IsFailure)
        {
            return Result.Failure(categoryUpdateResult.Error);
        }

        var subcategoryRemoveTask = _subcategoryRepository.RemoveAsync(subcategory, cancellationToken);
        var transactionsUpdateTask = _transactionRepository.RemoveRangeAsync(transactions, cancellationToken);

        var results = await Task.WhenAll(subcategoryRemoveTask, transactionsUpdateTask);

        var failureResult = results.FirstOrDefault(r => r.IsFailure);

        return failureResult ?? Result.Success();
    }
}