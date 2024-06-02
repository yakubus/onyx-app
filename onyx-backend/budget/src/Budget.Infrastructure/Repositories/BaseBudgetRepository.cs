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

    public override Result<TEntity> GetFirst(Expression<Func<TEntity, bool>> filterPredicate, CancellationToken cancellationToken = default)
    {
        var budgetIdGetResult = _budgetContext.GetBudgetId();

        if (budgetIdGetResult.IsFailure)
        {
            return budgetIdGetResult.Error;
        }

        var budgetId = new BudgetId(budgetIdGetResult.Value);

        var parameter = filterPredicate.Parameters[0];

        var budgetIdProperty = Expression.Property(parameter, nameof(BudgetOwnedEntity<TEntityId>.BudgetId));
        var budgetIdValue = Expression.Constant(budgetId);
        var budgetIdCondition = Expression.Equal(budgetIdProperty, budgetIdValue);

        var combinedExpression = Expression.Lambda<Func<TEntity, bool>>(
            Expression.AndAlso(filterPredicate.Body, budgetIdCondition),
            parameter);

        return base.GetFirst(combinedExpression, cancellationToken);
    }

    public override async Task<Result<IEnumerable<TEntity>>> GetAllAsync(CancellationToken cancellationToken)
    {
        var budgetIdGetResult = _budgetContext.GetBudgetId();

        if (budgetIdGetResult.IsFailure)
        {
            return budgetIdGetResult.Error;
        }

        var budgetId = new BudgetId(budgetIdGetResult.Value);

        //return Result.Create(await Task.Run(
        //    () => Table.GetItemLinqQueryable<TEntity>(true).Where(e => e.BudgetId == budgetId).AsEnumerable(),
        //    cancellationToken));
        return null;
    }

    public override Result<IEnumerable<TEntity>> GetWhere(Expression<Func<TEntity, bool>> filterPredicate, CancellationToken cancellationToken = default)
    {
        var budgetIdGetResult = _budgetContext.GetBudgetId();

        if (budgetIdGetResult.IsFailure)
        {
            return budgetIdGetResult.Error;
        }

        var budgetId = new BudgetId(budgetIdGetResult.Value);

        var parameter = filterPredicate.Parameters[0];

        var budgetIdProperty = Expression.Property(parameter, nameof(BudgetOwnedEntity<TEntityId>.BudgetId));
        var budgetIdValue = Expression.Constant(budgetId);
        var budgetIdCondition = Expression.Equal(budgetIdProperty, budgetIdValue);

        var combinedExpression = Expression.Lambda<Func<TEntity, bool>>(
            Expression.AndAlso(filterPredicate.Body, budgetIdCondition),
            parameter);

        return base.GetWhere(combinedExpression, cancellationToken);
    }
}