using System.Text.Json.Serialization;
using Abstractions.DomainBaseTypes;
using Models.Responses;

namespace Budget.Domain.Counterparties;

public abstract class Counterparty : Entity<CounterpartyId>
{
    public CounterpartyName Name { get; private set; }

    protected Counterparty(CounterpartyName name)
    {
        Name = name;
    }

    [JsonConstructor]
    protected Counterparty(){}

    public Result ChangeName(string name)
    {
        var counterpartyNameCreateResult = CounterpartyName.Create(name);

        if (counterpartyNameCreateResult.IsFailure)
        {
            return Result.Failure(counterpartyNameCreateResult.Error);
        }

        var counterpartyName = counterpartyNameCreateResult.Value;
        Name = counterpartyName;

        return Result.Success();
    }
}