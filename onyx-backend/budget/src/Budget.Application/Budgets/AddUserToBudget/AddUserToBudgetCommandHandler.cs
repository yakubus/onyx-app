using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abstractions.Messaging;
using Budget.Application.Abstractions.Identity;
using Budget.Application.Budgets.Models;
using Budget.Domain.Budgets;
using Models.Responses;

namespace Budget.Application.Budgets.AddUserToBudget;

internal sealed class AddUserToBudgetCommandHandler : ICommandHandler<AddUserToBudgetCommand, BudgetModel>
{
    private readonly IUserContext _userContext;
    private readonly IBudgetRepository _budgetRepository;

    public AddUserToBudgetCommandHandler(IUserContext userContext, IBudgetRepository budgetRepository)
    {
        _userContext = userContext;
        _budgetRepository = budgetRepository;
    }

    //TODO send event to update user
    public async Task<Result<BudgetModel>> Handle(AddUserToBudgetCommand request, CancellationToken cancellationToken)
    {
        var getUserIdResult = _userContext.GetUserId();

        if (getUserIdResult.IsFailure)
        {
            return getUserIdResult.Error;
        }

        var userId = getUserIdResult.Value;

        var getBudgetResult = await _budgetRepository.GetByIdAsync(new(request.BudgetId), cancellationToken);

        if (getUserIdResult.IsFailure)
        {
            return getBudgetResult.Error;
        }

        var budget = getBudgetResult.Value;

        var addUserResult = budget.AddUser(userId, request.Token);

        if (addUserResult.IsFailure)
        {
            return addUserResult.Error;
        }

        var updateResult = await _budgetRepository.UpdateAsync(budget, cancellationToken);

        if (updateResult.IsFailure)
        {
            return updateResult.Error;
        }

        budget = updateResult.Value;

        return BudgetModel.FromDomainModel(budget);
    }
}