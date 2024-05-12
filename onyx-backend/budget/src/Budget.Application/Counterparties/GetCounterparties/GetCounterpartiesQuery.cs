using Abstractions.Messaging;
using Budget.Application.Counterparties.Models;

namespace Budget.Application.Counterparties.GetCounterparties;

public sealed record GetCounterpartiesQuery(string CounterpartyType) : IQuery<IEnumerable<CounterpartyModel>>
{
}