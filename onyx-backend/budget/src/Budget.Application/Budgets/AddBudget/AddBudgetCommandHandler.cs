using Abstractions.Messaging;
using Budget.Application.Abstractions.Identity;
using Budget.Application.Budgets.Models;
using Budget.Domain.Budgets;
using Models.DataTypes;
using Models.Responses;

namespace Budget.Application.Budgets.AddBudget;

internal sealed class AddBudgetCommandHandler : ICommandHandler<AddBudgetCommand, BudgetModel>
{
    private readonly IBudgetRepository _budgetRepository;
    private readonly IUserContext _userContext;

    public AddBudgetCommandHandler(IBudgetRepository budgetRepository, IUserContext userContext)
    {
        _budgetRepository = budgetRepository;
        _userContext = userContext;
    }

    //TODO Send event
    public async Task<Result<BudgetModel>> Handle(AddBudgetCommand request, CancellationToken cancellationToken)
    {
        var isBudgetNameUnique = await _budgetRepository.GetByNameAsync(request.BudgetName, cancellationToken)
            is var getBudgetResult && getBudgetResult.IsFailure;

        if (!isBudgetNameUnique)
        {
            return AddBudgetErrors.BudgetNameNotUnique;
        }

        var userIdGetResult = _userContext.GetUserId();

        if (userIdGetResult.IsFailure)
        {
            return userIdGetResult.Error;
        }

        var userId = userIdGetResult.Value;

        var budgetCreateResult = Domain.Budgets.Budget.Create(request.BudgetName, userId, request.BudgetCurrency);

        if (budgetCreateResult.IsFailure)
        {
            return budgetCreateResult.Error;
        }

        var budget = budgetCreateResult.Value;

        var budgetAddResult = await _budgetRepository.AddAsync(budget, cancellationToken);

        if (budgetAddResult.IsFailure)
        {
            return budgetAddResult.Error;
        }

        budget = budgetAddResult.Value;

        return BudgetModel.FromDomainModel(budget);
    }
}