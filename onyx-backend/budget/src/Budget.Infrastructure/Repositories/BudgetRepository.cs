using Budget.Application.Abstractions.Identity;
using Budget.Domain.Budgets;
using Models.Responses;
using SharedDAL;

namespace Budget.Infrastructure.Repositories;

internal sealed class BudgetRepository : Repository<Domain.Budgets.Budget, BudgetId>, IBudgetRepository
{
    private readonly IBudgetContext _budgetContext;

    public BudgetRepository(CosmosDbContext context, IBudgetContext budgetContext) : base(context)
    {
            _budgetContext = budgetContext;
        }

    public Result<Domain.Budgets.Budget> GetByName(
        string name) =>
        GetFirst(b => b.Name.Value == name);

    public async Task<Result<Domain.Budgets.Budget>> GetCurrentBudget(CancellationToken cancellationToken)
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
        return await Task.Run(() => GetWhere(b => b.UserIds.Any(id => id == userId)), cancellationToken);
    }
}