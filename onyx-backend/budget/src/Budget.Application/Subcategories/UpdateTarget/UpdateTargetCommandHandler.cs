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

        var targetUpdateResult = currentTarget switch
        {
            null => subcategory.SetTarget(targetAmount, request.TargetUpToMonth),
            _ when currentTarget.TargetAmount != targetAmount => subcategory.UpdateTargetAmount(targetAmount),
            _ when currentTarget.UpToMonth != request.TargetUpToMonth => subcategory.MoveTargetEndMonth(
                request.TargetUpToMonth),
            _ => Result.Failure(Error.InvalidValue)
        };

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