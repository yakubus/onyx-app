using Models.Responses;

namespace Budget.Domain.Budgets;

public interface IBudgetRepository
{
    Task<Result<Budget>> GetByNameAsync(string name, CancellationToken cancellationToken);

    Task<Result<Budget>> AddAsync(Budget budget, CancellationToken cancellationToken);

    Task<Result<Budget>> GetByIdAsync(BudgetId budgetId, CancellationToken cancellationToken);

    Task<Result> RemoveAsync(Budget budget, CancellationToken cancellationToken);

    Task<Result<Budget>> UpdateAsync(Budget budget, CancellationToken cancellationToken);
}