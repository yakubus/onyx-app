using Budget.Application.Abstractions.Identity;
using Budget.Domain.Budgets;
using Microsoft.AspNetCore.Authorization;
using Models.Responses;

namespace Budget.Infrastructure.Authorization.BudgetMember;

internal class BudgetMemberAuthorizationHandler : AuthorizationHandler<BudgetMemberRequirement>
{
    private readonly AuthorizationErrorWriter _writer;
    private readonly IUserContext _userContext;
    private readonly IBudgetContext _budgetContext;
    private readonly IBudgetRepository _budgetRepository;

    public BudgetMemberAuthorizationHandler(IUserContext userContext, IBudgetContext budgetContext, IBudgetRepository budgetRepository, AuthorizationErrorWriter writer)
    {
        _userContext = userContext;
        _budgetContext = budgetContext;
        _budgetRepository = budgetRepository;
        _writer = writer;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, BudgetMemberRequirement requirement) =>
        _ = await IsBudgetMember() switch
        {
            { IsFailure: true } isBudgetMemberResult => await FailAsync(context, isBudgetMemberResult.Error),
            { IsSuccess: true, Value: false } => await FailAsync(
                context,
                AuthorizationErrors.BudgetMemberAuthorizationError),
            { IsSuccess: true, Value: true } => Succeed(context, requirement),
        };

    private async ValueTask<int> FailAsync(AuthorizationHandlerContext context, Error error)
    {
        await _writer.Write(error);
        context.Fail();
        return 0;
    }
    private int Succeed(AuthorizationHandlerContext context, BudgetMemberRequirement requirement)
    {
        context.Succeed(requirement);
        return 1;
    }

    private async ValueTask<Result<bool>> IsBudgetMember(CancellationToken cancellationToken = default)
    {
        var budgetIdGetResult = _budgetContext.GetBudgetId();

        if (budgetIdGetResult.IsFailure)
        {
            return budgetIdGetResult.Error;
        }

        var budgetId = budgetIdGetResult.Value;

        //var getUserBudgetIdsResult = _userContext.GetBudgetsIds();

        //if (getUserBudgetIdsResult.IsFailure)
        //{
        //    return getUserBudgetIdsResult.Error;
        //}

        //var isUserAMemberOfBudget = getUserBudgetIdsResult.Value.Any(id => id == budgetId);

        //if (isUserAMemberOfBudget)
        //{
        //    return true;
        //}

        var userIdGetResult = _userContext.GetUserId();

        if (userIdGetResult.IsFailure)
        {
            return userIdGetResult.Error;
        }

        var userId = userIdGetResult.Value;

        var userBudgetsGetResult = await _budgetRepository.GetBudgetsForUserAsync(userId, cancellationToken);

        if (userBudgetsGetResult.IsFailure)
        {
            return userBudgetsGetResult.Error;
        }

        var userBudgets = userBudgetsGetResult.Value;

        return userBudgets.Any(budget => budget.Id.Value == budgetId);
    }
}