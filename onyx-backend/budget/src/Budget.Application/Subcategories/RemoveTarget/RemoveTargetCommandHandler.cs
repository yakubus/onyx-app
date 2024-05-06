using Abstractions.Messaging;
using Budget.Domain.Subcategories;
using Models.Responses;

namespace Budget.Application.Subcategories.RemoveTarget;

internal sealed class RemoveTargetCommandHandler : ICommandHandler<RemoveTargetCommand>
{
    private readonly ISubcategoryRepository _subcategoryRepository;

    public RemoveTargetCommandHandler(ISubcategoryRepository subcategoryRepository)
    {
        _subcategoryRepository = subcategoryRepository;
    }

    public async Task<Result> Handle(RemoveTargetCommand request, CancellationToken cancellationToken)
    {
        var subcategoryId = new SubcategoryId(request.SubcategoryId);

        var subcategoryGetResult = await _subcategoryRepository.GetByIdAsync(subcategoryId, cancellationToken);

        if (subcategoryGetResult.IsFailure)
        {
            return Result.Failure(subcategoryGetResult.Error);
        }

        var subcategory = subcategoryGetResult.Value;

        var unsetTargetResult = subcategory.UnsetTarget();

        if (unsetTargetResult.IsFailure)
        {
            return Result.Failure(unsetTargetResult.Error);
        }

        var subcategoryUpdateResult = await _subcategoryRepository.UpdateAsync(subcategory, cancellationToken);

        if (subcategoryUpdateResult.IsFailure)
        {
            return Result.Failure(subcategoryUpdateResult.Error);
        }

        return Result.Success();
    }
}