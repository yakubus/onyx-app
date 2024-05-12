using Models.Responses;

namespace Budget.Application.Counterparties.AddCounterparty;

internal static class AddCounterpartyErrors
{
    internal static readonly Error CounterpartyAlreadyExists = new (
        "Counterparty.AlreadyExists",
        "Counterparty already exists");
}