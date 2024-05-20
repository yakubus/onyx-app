using Abstractions.Messaging;
using Budget.Application.Subcategories.Models;
using Budget.Domain.Subcategories;
using Models.Responses;

namespace Budget.Application.Subcategories.UpdateTarget;

internal sealed class UpdateTargetCommandHandler : ICommandHandler<UpdateTargetCommand, SubcategoryModel>
{
    private readonly ISubcategoryRepository _subcategoryRepository;

    public UpdateTargetCommandHandler(ISubcategoryRepository subcategoryRepository)
    {
        _subcategoryRepository = subcategoryRepository;
    }
    //TODO Money Exchange
    public async Task<Result<SubcategoryModel>> Handle(UpdateTargetCommand request, CancellationToken cancellationToken)
    {
        var subcategoryId = new SubcategoryId(request.SubcategoryId);
        
        var subcategoryGetResult = await _subcategoryRepository.GetByIdAsync(subcategoryId, cancellationToken);

        if (subcategoryGetResult.IsFailure)
        {
            return Result.Failure<SubcategoryModel>(subcategoryGetResult.Error);
        }

        var subcategory = subcategoryGetResult.Value;

        var targetAmountCreateResult = request.TargetAmount.ToDomainModel();

        if (targetAmountCreateResult.IsFailure)
        {
            return Result.Failure<SubcategoryModel>(targetAmountCreateResult.Error);
        }

        var targetAmount = targetAmountCreateResult.Value;
        var currentTarget = subcategory.Target;

        var targetUpdateResult = SubcategoryService.UpdateTarget(
            subcategory,
            currentTarget,
            targetAmount,
            request.TargetUpToMonth);

        if (targetUpdateResult.IsFailure)
        {
            return Result.Failure<SubcategoryModel>(targetUpdateResult.Error);
        }

        var subcategoryUpdateResult = await _subcategoryRepository.UpdateAsync(subcategory, cancellationToken);

        if (subcategoryUpdateResult.IsFailure)
        {
            return Result.Failure<SubcategoryModel>(subcategoryUpdateResult.Error);
        }

        subcategory = subcategoryUpdateResult.Value;

        return SubcategoryModel.FromDomainModel(subcategory);
    }
}