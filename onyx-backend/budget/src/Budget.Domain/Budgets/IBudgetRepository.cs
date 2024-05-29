using Models.Responses;

namespace Budget.Domain.Budgets;

public interface IBudgetRepository
{
    Result<Budget> GetByName(string name);

    Task<Result<Budget>> AddAsync(Budget budget, CancellationToken cancellationToken);

    Task<Result<Budget>> GetByIdAsync(BudgetId budgetId, CancellationToken cancellationToken);

    Task<Result> RemoveAsync(BudgetId budgetId, CancellationToken cancellationToken);

    Task<Result<Budget>> UpdateAsync(Budget budget, CancellationToken cancellationToken);

    Task<Result<Budget>> GetCurrentBudget(CancellationToken cancellationToken);

    Task<Result<IEnumerable<Budget>>> GetBudgetsForUserAsync(string userId, CancellationToken cancellationToken);
}