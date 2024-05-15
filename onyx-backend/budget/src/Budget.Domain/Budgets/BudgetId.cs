using Abstractions.DomainBaseTypes;
using Budget.Domain.Converters.EntityIdConverters;
using Newtonsoft.Json;

namespace Budget.Domain.Budgets;

[JsonConverter(typeof(BudgetIdConverter))]
public sealed record BudgetId : EntityId
{
    public BudgetId()
    { }

    public BudgetId(string value) : base(value)
    { }

    public BudgetId(Guid value) : base(value)
    { }
}