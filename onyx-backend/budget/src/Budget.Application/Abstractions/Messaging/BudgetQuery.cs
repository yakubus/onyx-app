using Abstractions.Messaging;

namespace Budget.Application.Abstractions.Messaging;

public abstract record BudgetQuery<TResponse>(Guid BudgetId) : IQuery<TResponse>;