using Abstractions.Messaging;
using Budget.Application.Abstractions.Messaging;

namespace Budget.Application.Counterparties.RemoveCounterparty;

public sealed record RemoveCounterpartyCommand(Guid Id, Guid BudgetId) : BudgetCommand(BudgetId)
{
}