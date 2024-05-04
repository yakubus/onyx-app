using Models.Responses;

namespace Budget.Domain.Counterparties;

internal static class CounterpartyErrors
{
    internal static readonly Error InvalidName = new(
        "Counterparty.Message.InvalidValue",
        "Invalid counterparty name");
}