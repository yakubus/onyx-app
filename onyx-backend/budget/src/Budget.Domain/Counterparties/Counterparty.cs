using Abstractions.DomainBaseTypes;
using Models.Responses;

namespace Budget.Domain.Counterparties;

public sealed class Counterparty : Entity<CounterpartyId>
{
    public CounterpartyName Name { get; private set; }
    public CounterpartyType Type { get; init; }

    [Newtonsoft.Json.JsonConstructor]
    [System.Text.Json.Serialization.JsonConstructor]
    private Counterparty(CounterpartyName name, CounterpartyType type) : base(new CounterpartyId())
    {
        Name = name;
        Type = type;
    }

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

    public static Result<Counterparty> Create(string name, string type)
    {
        var counterpartyNameCreateResult = CounterpartyName.Create(name);

        if (counterpartyNameCreateResult.IsFailure)
        {
            return Result.Failure<Counterparty>(counterpartyNameCreateResult.Error);
        }

        var counterpartyName = counterpartyNameCreateResult.Value;

        var counterpartyTypeCreateResult = CounterpartyType.Create(type);

        if (counterpartyTypeCreateResult.IsFailure)
        {
            return Result.Failure<Counterparty>(counterpartyTypeCreateResult.Error);
        }

        var counterpartyType = counterpartyTypeCreateResult.Value;

        return new Counterparty(counterpartyName, counterpartyType);
    }
}