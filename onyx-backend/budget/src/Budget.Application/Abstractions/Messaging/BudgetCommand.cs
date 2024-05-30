using Abstractions.Messaging;

namespace Budget.Application.Abstractions.Messaging;

public abstract record BudgetCommand<TResponse>(Guid BudgetId) : ICommand<TResponse>;
public abstract record BudgetCommand(Guid BudgetId) : ICommand;