using Abstractions.Messaging;
using Budget.Application.Counterparties.Models;

namespace Budget.Application.Counterparties.AddCounterparty;

public sealed record AddCounterpartyCommand(string CounterpartyType, string CounterpartyName) : ICommand<CounterpartyModel>
{
}