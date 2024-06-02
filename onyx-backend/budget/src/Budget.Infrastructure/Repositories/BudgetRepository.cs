using Budget.Application.Abstractions.Identity;
using Budget.Domain.Budgets;
using Models.Responses;
using SharedDAL;
using SharedDAL.DataModels.Abstractions;

namespace Budget.Infrastructure.Repositories;

internal sealed class BudgetRepository : Repository<Domain.Budgets.Budget, BudgetId>, IBudgetRepository
{
    private readonly IBudgetContext _budgetContext;

    public BudgetRepository(
        DbContext context,
        IBudgetContext budgetContext,
        IDataModelService<Domain.Budgets.Budget> dataModelService) : base(
        context,
        dataModelService)
    {
        _budgetContext = budgetContext;
    }

    public async Task<Result<Domain.Budgets.Budget>> GetByNameAsync(
        string name,
        CancellationToken cancellationToken) =>
        await GetFirst($"Name = '{name}'", cancellationToken);

    public async Task<Result<Domain.Budgets.Budget>> GetCurrentBudgetAsync(CancellationToken cancellationToken)
    {
        var budgetIdGetResult = _budgetContext.GetBudgetId();

        if (budgetIdGetResult.IsFailure)
        {
            return budgetIdGetResult.Error;
        }

        var budgetId = new BudgetId(budgetIdGetResult.Value);

        return await GetByIdAsync(budgetId, cancellationToken);
    }

    public async Task<Result<IEnumerable<Domain.Budgets.Budget>>> GetBudgetsForUserAsync(string userId, CancellationToken cancellationToken)
    {
        return await GetWhere($"CONTAINS (UserIds, '{userId}')", cancellationToken);
    }
}