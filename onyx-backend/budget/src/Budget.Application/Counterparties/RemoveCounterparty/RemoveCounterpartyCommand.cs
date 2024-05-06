using Abstractions.Messaging;

namespace Budget.Application.Counterparties.RemoveCounterparty;

public sealed record RemoveCounterpartyCommand(Guid Id) : ICommand
{
}