using Models.Responses;
using System.Text.Json.Serialization;

namespace Budget.Domain.Counterparties.Payees;

public sealed class Payee : Counterparty
{
    private Payee(CounterpartyName name) : base(name)
    {
    }


    [JsonConstructor]
    private Payee() { }

    public static Result<Payee> Create(string name)
    {
        var counterpartyNameCreateResult = CounterpartyName.Create(name);

        if (counterpartyNameCreateResult.IsFailure)
        {
            return Result.Failure<Payee>(counterpartyNameCreateResult.Error);
        }

        var counterpartyName = counterpartyNameCreateResult.Value;

        return new Payee(counterpartyName);
    }
}