using System.Diagnostics.CodeAnalysis;
using System.Net.Sockets;
using Abstractions.Messaging;
using Budget.Application.Budgets.Models;
using Budget.Domain.Budgets;
using Models.Responses;

namespace Budget.Application.Budgets.UpdateBudget;

internal sealed class UpdateBudgetCommandHandler : ICommandHandler<UpdateBudgetCommand, BudgetModel>
{
    private readonly IBudgetRepository _repository;

    public UpdateBudgetCommandHandler(IBudgetRepository repository)
    {
        _repository = repository;
    }


    public async Task<Result<BudgetModel>> Handle(UpdateBudgetCommand request, CancellationToken cancellationToken)
    {
        var budgetId = new BudgetId(request.BudgetId);
        var getBudgetResult = await _repository.GetByIdAsync(budgetId, cancellationToken);

        if (getBudgetResult.IsFailure)
        {
            return getBudgetResult.Error;
        }

        var budget = getBudgetResult.Value;

        var updateBudgetResult = (
                string.IsNullOrWhiteSpace(request.UserIdToAdd),
                string.IsNullOrWhiteSpace(request.UserIdToRemove)
                ) switch
        {
            (false, true) => budget.AddUser(request.UserIdToAdd!),
            (true, false) => budget.ExcludeUser(request.UserIdToRemove!),
            _ => Error.InvalidValue
        };

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