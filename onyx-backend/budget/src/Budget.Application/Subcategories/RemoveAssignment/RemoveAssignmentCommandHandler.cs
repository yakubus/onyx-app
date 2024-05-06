using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abstractions.Messaging;
using Budget.Domain.Subcategories;
using Models.Responses;

namespace Budget.Application.Subcategories.RemoveAssignment;

internal sealed class RemoveAssignmentCommandHandler : ICommandHandler<RemoveAssignmentCommand>
{
    private readonly ISubcategoryRepository _subcategoryRepository;

    public RemoveAssignmentCommandHandler(ISubcategoryRepository subcategoryRepository)
    {
        _subcategoryRepository = subcategoryRepository;
    }

    public async Task<Result> Handle(RemoveAssignmentCommand request, CancellationToken cancellationToken)
    {
        var subcategoryId = new SubcategoryId(request.SubcategoryId);

        var subcategoryGetResult = await _subcategoryRepository.GetByIdAsync(subcategoryId, cancellationToken);

        if (subcategoryGetResult.IsFailure)
        {
            return Result.Failure(subcategoryGetResult.Error);
        }

        var subcategory = subcategoryGetResult.Value;

        var unassignResult = subcategory.Unassign(request.AssignmentMonth.Month, request.AssignmentMonth.Month);

        if (unassignResult.IsFailure)
        {
            return Result.Failure(unassignResult.Error);
        }

        var updateResult = await _subcategoryRepository.UpdateAsync(subcategory, cancellationToken);

        if (updateResult.IsFailure)
        {
            return Result.Failure(updateResult.Error);
        }

        return Result.Success();
    }
}