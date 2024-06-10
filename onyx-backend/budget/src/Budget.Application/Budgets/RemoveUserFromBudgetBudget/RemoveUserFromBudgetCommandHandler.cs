using Abstractions.Messaging;
using Budget.Application.Budgets.Models;
using Budget.Domain.Budgets;
using Models.Responses;

namespace Budget.Application.Budgets.UpdateBudget;

internal sealed class RemoveUserFromBudgetCommandHandler : ICommandHandler<RemoveUserFromBudgetCommand, BudgetModel>
{
    private readonly IBudgetRepository _repository;
    private static readonly Error _invalidInputError = new Error(
        "UpdateBudget.InvalidInput", 
        "Specify either user to add or user to remove"
        );

    public RemoveUserFromBudgetCommandHandler(IBudgetRepository repository)
    {
        _repository = repository;
    }

    //TODO Send event when user added or removed
    public async Task<Result<BudgetModel>> Handle(RemoveUserFromBudgetCommand request, CancellationToken cancellationToken)
    {
        var budgetId = new BudgetId(request.BudgetId);
        var getBudgetResult = await _repository.GetByIdAsync(budgetId, cancellationToken);

        if (getBudgetResult.IsFailure)
        {
            return getBudgetResult.Error;
        }

        var budget = getBudgetResult.Value;

        var updateBudgetResult = budget.ExcludeUser(request.UserIdToRemove);

        if (updateBudgetResult.IsFailure)
        {
            return updateBudgetResult.Error;
        }

        var updateResult = await _repository.UpdateAsync(budget, cancellationToken);

        if (updateResult.IsFailure)
        {
            return updateResult.Error;
        }

        budget = updateResult.Value;

        return BudgetModel.FromDomainModel(budget);
    }
}