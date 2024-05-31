using Budget.Application.Abstractions.Messaging;

namespace Budget.Application.Accounts.RemoveAccount;

public sealed record RemoveAccountCommand(Guid Id, Guid BudgetId) : BudgetCommand(BudgetId)
{
}