using Abstractions.Messaging;
using Budget.Application.Abstractions.Messaging;
using Budget.Application.Counterparties.Models;

namespace Budget.Application.Counterparties.UpdateCounterparty;

public sealed record UpdateCounterpartyCommand(
    Guid Id,
    string NewName,
    Guid BudgetId) : BudgetCommand<CounterpartyModel>(BudgetId)
{
}