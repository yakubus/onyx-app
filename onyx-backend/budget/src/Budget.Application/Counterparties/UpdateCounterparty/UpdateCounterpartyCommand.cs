using Abstractions.Messaging;
using Budget.Application.Counterparties.Models;

namespace Budget.Application.Counterparties.UpdateCounterparty;

public sealed record UpdateCounterpartyCommand(Guid Id, string Name) : ICommand<CounterpartyModel>
{
}