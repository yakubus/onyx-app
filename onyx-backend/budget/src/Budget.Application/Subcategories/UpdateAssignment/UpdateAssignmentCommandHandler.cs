using Abstractions.Messaging;
using Budget.Application.Subcategories.Models;
using Budget.Domain.Subcategories;
using Models.Responses;

namespace Budget.Application.Subcategories.UpdateAssignment;

internal sealed class UpdateAssignmentCommandHandler : ICommandHandler<UpdateAssignmentCommand, SubcategoryModel>
{
    private readonly ISubcategoryRepository _subcategoryRepository;

    public UpdateAssignmentCommandHandler(ISubcategoryRepository subcategoryRepository)
    {
        _subcategoryRepository = subcategoryRepository;
    }
    //TODO Money Exchange
    public async Task<Result<SubcategoryModel>> Handle(UpdateAssignmentCommand request, CancellationToken cancellationToken)
    {
        var subcategoryId = new SubcategoryId(request.SubcategoryId);

        var subcategoryGetResult = await _subcategoryRepository.GetByIdAsync(subcategoryId, cancellationToken);

        if (subcategoryGetResult.IsFailure)
        {
            return Result.Failure<SubcategoryModel>(subcategoryGetResult.Error);
        }

        var subcategory = subcategoryGetResult.Value;

        var assignedAmountMoneyCreateResult = request.AssignedAmount.ToDomainModel();

        if (assignedAmountMoneyCreateResult.IsFailure)
        {
            return Result.Failure<SubcategoryModel>(assignedAmountMoneyCreateResult.Error);
        }

        var assignedAmountMoney = assignedAmountMoneyCreateResult.Value;

        var assignmentResult = SubcategoryService.UpdateAssignment(
            subcategory,
            request.AssignmentMonth,
            assignedAmountMoney);

        if (assignmentResult.IsFailure)
        {
            return Result.Failure<SubcategoryModel>(assignmentResult.Error);
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