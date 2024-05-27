using Abstractions.DomainBaseTypes;
using Budget.Domain.Budgets;

namespace Budget.Domain.Shared.Abstractions;

public abstract class BudgetOwnedEntity<TId> : Entity<TId> 
    where TId : EntityId, new()
{
    public BudgetId BudgetId { get; init; }

    [System.Text.Json.Serialization.JsonConstructor]
    [Newtonsoft.Json.JsonConstructor]
    protected BudgetOwnedEntity(BudgetId budgetId, TId id) : base(id)
    {
        BudgetId = budgetId;
    }
}