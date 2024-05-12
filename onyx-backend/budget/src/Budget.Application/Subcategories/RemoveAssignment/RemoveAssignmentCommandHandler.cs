using Abstractions.Messaging;
using Budget.Application.Subcategories.Models;
using Budget.Domain.Subcategories;
using Models.Responses;

namespace Budget.Application.Subcategories.RemoveAssignment;

internal sealed class RemoveAssignmentCommandHandler : ICommandHandler<RemoveAssignmentCommand, SubcategoryModel>
{
    private readonly ISubcategoryRepository _subcategoryRepository;

    public RemoveAssignmentCommandHandler(ISubcategoryRepository subcategoryRepository)
    {
        _subcategoryRepository = subcategoryRepository;
    }

    public async Task<Result<SubcategoryModel>> Handle(RemoveAssignmentCommand request, CancellationToken cancellationToken)
    {
        var subcategoryId = new SubcategoryId(request.SubcategoryId);

        var subcategoryGetResult = await _subcategoryRepository.GetByIdAsync(subcategoryId, cancellationToken);

        if (subcategoryGetResult.IsFailure)
        {
            return Result.Failure<SubcategoryModel>(subcategoryGetResult.Error);
        }

        var subcategory = subcategoryGetResult.Value;

        var unassignResult = subcategory.Unassign(request.AssignmentMonth.Month, request.AssignmentMonth.Year);

        if (unassignResult.IsFailure)
        {
            return Result.Failure<SubcategoryModel>(unassignResult.Error);
        }

        var updateResult = await _subcategoryRepository.UpdateAsync(subcategory, cancellationToken);

        if (updateResult.IsFailure)
        {
            return Result.Failure<SubcategoryModel>(updateResult.Error);
        }

        subcategory = updateResult.Value;

        return SubcategoryModel.FromDomainModel(subcategory);
    }
}