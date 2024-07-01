using Abstractions.DomainBaseTypes;
using Amazon.DynamoDBv2.DocumentModel;
using Budget.Application.Abstractions.Identity;
using Budget.Domain.Budgets;
using Budget.Domain.Shared.Abstractions;
using Models.Responses;
using SharedDAL;
using SharedDAL.DataModels.Abstractions;

namespace Budget.Infrastructure.Repositories;

internal abstract class BaseBudgetRepository<TEntity, TEntityId> : Repository<TEntity, TEntityId>
    where TEntityId : EntityId, new() where TEntity : BudgetOwnedEntity<TEntityId>
{
    private readonly IBudgetContext _budgetContext;

    protected BaseBudgetRepository(
        DbContext context,
        IBudgetContext budgetContext,
        IDataModelService<TEntity> dataModelService) : base(
        context,
        dataModelService)
    {
        _budgetContext = budgetContext;
    }

    protected override async Task<Result<TEntity>> GetFirstAsync(ScanFilter scanFilter, CancellationToken cancellationToken = default)
    {
        var budgetIdGetResult = _budgetContext.GetBudgetId();

        if (budgetIdGetResult.IsFailure)
        {
            return budgetIdGetResult.Error;
        }

        var budgetId = new BudgetId(budgetIdGetResult.Value);

        scanFilter.AddCondition("BudgetId", ScanOperator.Equal, budgetId.Value.ToString());

        return await base.GetFirstAsync(scanFilter, cancellationToken);
    }

    public override async Task<Result<IEnumerable<TEntity>>> GetAllAsync(CancellationToken cancellationToken)
    {
        var budgetIdGetResult = _budgetContext.GetBudgetId();

        if (budgetIdGetResult.IsFailure)
        {
            return budgetIdGetResult.Error;
        }

        var budgetId = new BudgetId(budgetIdGetResult.Value);

        var scanFilter = new ScanFilter();
        scanFilter.AddCondition("BudgetId", ScanOperator.Equal, budgetId.Value.ToString());

        return await GetWhereAsync(scanFilter, cancellationToken);
    }

    protected override async Task<Result<IEnumerable<TEntity>>> GetWhereAsync(
        ScanFilter scanFilter,
        CancellationToken cancellationToken = default)
    {
        var budgetIdGetResult = _budgetContext.GetBudgetId();

        if (budgetIdGetResult.IsFailure)
        {
            return budgetIdGetResult.Error;
        }

        var budgetId = new BudgetId(budgetIdGetResult.Value);

        scanFilter.AddCondition("BudgetId", ScanOperator.Equal, budgetId.Value.ToString());

        return await base.GetWhereAsync(scanFilter, cancellationToken);
    }
}