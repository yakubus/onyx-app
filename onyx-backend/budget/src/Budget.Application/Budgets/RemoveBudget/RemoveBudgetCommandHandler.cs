using Abstractions.Messaging;
using Budget.Domain.Budgets;
using Models.Responses;

namespace Budget.Application.Budgets.RemoveBudget;

internal sealed class RemoveBudgetCommandHandler : ICommandHandler<RemoveBudgetCommand>
{
    private readonly IBudgetRepository _budgetRepository;

    public RemoveBudgetCommandHandler(IBudgetRepository budgetRepository)
    {
        _budgetRepository = budgetRepository;
    }

    //TODO Consider retrieving budget from jwt
    public async Task<Result> Handle(RemoveBudgetCommand request, CancellationToken cancellationToken)
    {
        var budgetId = new BudgetId(request.BudgetId);  
        var budgetGetResult = await _budgetRepository.GetByIdAsync(budgetId, cancellationToken);

        if (budgetGetResult.IsFailure)
        {
            return budgetGetResult.Error;
        }

        var budget = budgetGetResult.Value;

        return await _budgetRepository.RemoveAsync(budget, cancellationToken);
    }
}