using Abstractions.Messaging;
using Budget.Domain.Categories;
using Budget.Domain.Subcategories;
using Budget.Domain.Transactions;
using Models.Responses;

namespace Budget.Application.Subcategories.RemoveSubcategory;

internal sealed class RemoveSubcategoryCommandHandler : ICommandHandler<RemoveSubcategoryCommand>
{
    private readonly ISubcategoryRepository _subcategoryRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly ITransactionRepository _transactionRepository;

    public RemoveSubcategoryCommandHandler(ISubcategoryRepository subcategoryRepository, ICategoryRepository categoryRepository, ITransactionRepository transactionRepository)
    {
        _subcategoryRepository = subcategoryRepository;
        _categoryRepository = categoryRepository;
        _transactionRepository = transactionRepository;
    }

    public async Task<Result> Handle(RemoveSubcategoryCommand request, CancellationToken cancellationToken)
    {
        var subcategoryId = new SubcategoryId(request.Id);

        var subcategoryGetResult = await _subcategoryRepository.GetByIdAsync(subcategoryId, cancellationToken);

        if (subcategoryGetResult.IsFailure)
        {
            return subcategoryGetResult.Error;
        }

        var subcategory = subcategoryGetResult.Value;

        var categoryGetResult = await _categoryRepository.GetCategoryWithSubcategory(
            subcategory.Id,
            cancellationToken);

        if (categoryGetResult.IsFailure)
        {
            return categoryGetResult.Error;
        }

        var relatedTransactionsGetResult = await _transactionRepository.GetBySubcategoryAsync(
            subcategoryId,
            cancellationToken);

        if (relatedTransactionsGetResult.IsFailure)
        {
            return relatedTransactionsGetResult.Error;
        }

        var relatedTransactions = relatedTransactionsGetResult.Value.ToList();
        var category = categoryGetResult.Value;

        var subcategoryServiceRemoveResult = SubcategoryService.RemoveSubcategory(
            subcategory, 
            category, 
            relatedTransactions);

        if (subcategoryServiceRemoveResult.IsFailure)
        {
            return subcategoryServiceRemoveResult.Error;
        }

        var categoryUpdateResult = await _categoryRepository.UpdateAsync(category, cancellationToken);

        if (categoryUpdateResult.IsFailure)
        {
            return categoryUpdateResult.Error;
        }

        var transactionsUpdateResult =
            await _transactionRepository.UpdateRangeAsync(relatedTransactions, cancellationToken);

        if (transactionsUpdateResult.IsFailure)
        {
            return transactionsUpdateResult.Error;
        }

        var subcategoryRemoveResult = await _subcategoryRepository.RemoveAsync(subcategory.Id, cancellationToken);

        if (subcategoryRemoveResult.IsFailure)
        {
            return subcategoryRemoveResult.Error;
        }

        return Result.Success();
    }
}