using Abstractions.Messaging;
using Budget.Application.Subcategories.Models;
using Budget.Domain.Subcategories;
using Budget.Domain.Transactions;
using Models.DataTypes;
using Models.Responses;

namespace Budget.Application.Subcategories.UpdateTarget;

internal sealed class UpdateTargetCommandHandler : ICommandHandler<UpdateTargetCommand, SubcategoryModel>
{
    private readonly ISubcategoryRepository _subcategoryRepository;
    private readonly ITransactionRepository _transactionRepository;

    public UpdateTargetCommandHandler(ISubcategoryRepository subcategoryRepository, ITransactionRepository transactionRepository)
    {
        _subcategoryRepository = subcategoryRepository;
        _transactionRepository = transactionRepository;
    }

    public async Task<Result<SubcategoryModel>> Handle(UpdateTargetCommand request, CancellationToken cancellationToken)
    {
        var subcategoryId = new SubcategoryId(request.SubcategoryId);
        
        var subcategoryGetResult = await _subcategoryRepository.GetByIdAsync(subcategoryId, cancellationToken);

        if (subcategoryGetResult.IsFailure)
        {
            return Result.Failure<SubcategoryModel>(subcategoryGetResult.Error);
        }

        var subcategory = subcategoryGetResult.Value;

        var budgetCurrency = Currency.Usd; //TODO Fix when authentication implemented

        var targetAmount = new Money(request.TargetAmount, budgetCurrency);
        var currentTarget = subcategory.Target;
        var isNewTarget = currentTarget is null;

        var targetUpdateResult = SubcategoryService.UpdateTarget(
            subcategory,
            currentTarget,
            targetAmount,
            request.StartedAt,
            request.TargetUpToMonth);

        if (targetUpdateResult.IsFailure)
        {
            return Result.Failure<SubcategoryModel>(targetUpdateResult.Error);
        }

        if (!isNewTarget)
        {
            return await _subcategoryRepository.UpdateAsync(subcategory, cancellationToken) is var result &&
                   result.IsFailure ?
                result.Error :
                SubcategoryModel.FromDomainModel(result.Value);
        }

        var newTarget = subcategory.Target!;

        var targetTransactResult = AddExistingTransactionsForTarget(subcategory, newTarget, cancellationToken);

        if (targetTransactResult.IsFailure)
        {
            return targetTransactResult.Error;
        }

        var subcategoryUpdateResult = await _subcategoryRepository.UpdateAsync(subcategory, cancellationToken);

        if (subcategoryUpdateResult.IsFailure)
        {
            return subcategoryUpdateResult.Error;
        }

        return Result.Success(SubcategoryModel.FromDomainModel(subcategory));
    }


    private Result AddExistingTransactionsForTarget(
        Subcategory subcategory,
        Target target,
        CancellationToken cancellationToken)
    {
        var relatedTransactionsGetResult = _transactionRepository.GetWhere(
            t => t.SubcategoryId != null &&
                 t.SubcategoryId == subcategory.Id &&
                 t.TransactedAt.Month >= target.StartedAt.Month &&
                 t.TransactedAt.Year >= target.StartedAt.Year &&
                 t.TransactedAt.Month <= target.UpToMonth.Month &&
                 t.TransactedAt.Year <= target.UpToMonth.Year,
            cancellationToken);

        if (relatedTransactionsGetResult.IsFailure)
        {
            return relatedTransactionsGetResult.Error;
        }

        var relatedTransactions = relatedTransactionsGetResult.Value.ToArray();

        var targetTransactResults = relatedTransactions.Select(subcategory.TransactForTarget);

        if (Result.Aggregate(targetTransactResults) is var result && result.IsFailure)
        {
            return result.Error;
        }

        return Result.Success();
    }
}