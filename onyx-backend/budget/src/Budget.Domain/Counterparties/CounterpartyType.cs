using Models.Responses;

namespace Budget.Domain.Counterparties;

public sealed record CounterpartyType
{
    public string Value { get; init; }

    [Newtonsoft.Json.JsonConstructor]
    [System.Text.Json.Serialization.JsonConstructor]
    private CounterpartyType(string value) => Value = value;

    public static readonly CounterpartyType Payee = new("Payee");
    public static readonly CounterpartyType Payer = new("Payer");
    public static readonly IReadOnlyCollection<CounterpartyType> All = new List<CounterpartyType>
    {
        Payee,
        Payer
    };

    private static readonly Error invalidAccountTypeError = new(
        "CounterpartyType.InvalidValue",
        "Invalid counterparty type");

    public static Result<CounterpartyType> Create(string value) =>
        All.FirstOrDefault(
            type => string.Equals(
                type.Value,
                value,
                StringComparison.CurrentCultureIgnoreCase)) ??
        Result.Failure<CounterpartyType>(invalidAccountTypeError);
}