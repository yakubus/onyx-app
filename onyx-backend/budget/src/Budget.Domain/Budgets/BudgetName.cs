using System.Text.RegularExpressions;
using Abstractions.DomainBaseTypes;
using Models.Responses;

namespace Budget.Domain.Budgets;

public sealed record BudgetName : ValueObject
{
    public string Value { get; private set; }
    private static readonly Regex valuePattern = new(@"^[a-zA-Z0-9\s.-]{1,50}$");

    [Newtonsoft.Json.JsonConstructor]
    [System.Text.Json.Serialization.JsonConstructor]
    private BudgetName(string value) => Value = value;

    public static Result<BudgetName> Create(string value)
    {
        value = value.Trim();

        return valuePattern.IsMatch(value) ?
            new BudgetName(value) :
            Result.Failure<BudgetName>(BudgetErrors.InvalidNameError);
    }
}