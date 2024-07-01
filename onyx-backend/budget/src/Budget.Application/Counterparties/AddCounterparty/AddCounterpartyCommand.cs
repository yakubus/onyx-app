using Budget.Application.Abstractions.Messaging;
using Budget.Application.Counterparties.Models;

namespace Budget.Application.Counterparties.AddCounterparty;

public sealed record AddCounterpartyCommand(
    string CounterpartyType,
    string CounterpartyName,
    Guid BudgetId) : BudgetCommand<CounterpartyModel>(BudgetId);