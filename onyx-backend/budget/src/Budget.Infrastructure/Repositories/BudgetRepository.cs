using Amazon.DynamoDBv2.DocumentModel;
using Budget.Application.Abstractions.Identity;
using Budget.Domain.Budgets;
using Budget.Infrastructure.Data.DataModels.Budgets;
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
        var scanFilter = new ScanFilter();
        scanFilter.AddCondition(nameof(BudgetDataModel.UserIds), ScanOperator.Contains, userId);

        return await GetWhereAsync(scanFilter, cancellationToken);
    }
}