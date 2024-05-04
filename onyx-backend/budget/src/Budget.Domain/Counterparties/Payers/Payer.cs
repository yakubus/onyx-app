using Models.Responses;
using System.Text.Json.Serialization;

namespace Budget.Domain.Counterparties.Payers;

public sealed class Payer : Counterparty
{
    private Payer(CounterpartyName name) : base(name)
    {
    }

    [JsonConstructor]
    private Payer() { }

    public static Result<Payer> Create(string name)
    {
        var counterpartyNameCreateResult = CounterpartyName.Create(name);

        if (counterpartyNameCreateResult.IsFailure)
        {
            return Result.Failure<Payer>(counterpartyNameCreateResult.Error);
        }

        var counterpartyName = counterpartyNameCreateResult.Value;

        return new Payer(counterpartyName);
    }
}