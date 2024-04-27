using System.Text.RegularExpressions;
using Abstractions.DomainBaseTypes;
using Extensions.Formatters;
using Models.Responses;

namespace Budget.Domain.Counterparties;

public sealed record CounterpartyName : ValueObject
{
    public string Value { get; init; }
    private static readonly Regex valuePattern = new(@"^[a-zA-Z0-9\s.-]{1,50}$");

    private CounterpartyName(string value) => Value = value;

    public static Result<CounterpartyName> Create(string value)
    {
        value = value.Trim().Capitalize();

        return valuePattern.IsMatch(value) ?
            new CounterpartyName(value) :
            Result.Failure<CounterpartyName>(CounterpartyErrors.InvalidName);
    }
}