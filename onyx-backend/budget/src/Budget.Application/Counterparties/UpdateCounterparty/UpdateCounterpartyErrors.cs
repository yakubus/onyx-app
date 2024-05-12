using Models.Responses;

namespace Budget.Application.Counterparties.UpdateCounterparty;

internal static class UpdateCounterpartyErrors
{
    internal static readonly Error CounterpartyAlreadyExists = new (
        "Counterparty.AlreadyExists",
        "Counterparty already exists");
}