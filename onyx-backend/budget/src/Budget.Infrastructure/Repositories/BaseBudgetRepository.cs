using System.Linq.Expressions;
using Abstractions.DomainBaseTypes;
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

    public override async Task<Result<TEntity>> GetFirst(string query, CancellationToken cancellationToken = default)
    {
        var budgetIdGetResult = _budgetContext.GetBudgetId();

        if (budgetIdGetResult.IsFailure)
        {
            return budgetIdGetResult.Error;
        }

        var budgetId = new BudgetId(budgetIdGetResult.Value);

        var combinedQuery = string.Join(query, " ", $"AND BudgetId = '{budgetId.Value}'");

        return await base.GetFirst(combinedQuery, cancellationToken);
    }

    public override async Task<Result<IEnumerable<TEntity>>> GetAllAsync(CancellationToken cancellationToken)
    {
        var budgetIdGetResult = _budgetContext.GetBudgetId();

        if (budgetIdGetResult.IsFailure)
        {
            return budgetIdGetResult.Error;
        }

        var budgetId = new BudgetId(budgetIdGetResult.Value);

        return await GetWhere($"BudgetId = '{budgetId.Value}'", cancellationToken);
    }

    public override async Task<Result<IEnumerable<TEntity>>> GetWhere(
        string query,
        CancellationToken cancellationToken = default)
    {
        var budgetIdGetResult = _budgetContext.GetBudgetId();

        if (budgetIdGetResult.IsFailure)
        {
            return budgetIdGetResult.Error;
        }

        var budgetId = new BudgetId(budgetIdGetResult.Value);

        var combinedQuery = string.Join(query, " ", $"AND BudgetId = '{budgetId.Value}'");

        return await base.GetWhere(combinedQuery, cancellationToken);
    }
}