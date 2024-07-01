using System.Text.RegularExpressions;
using Abstractions.DomainBaseTypes;
using Extensions.Formatters;
using Models.Responses;

namespace Budget.Domain.Categories;

public sealed record CategoryName : ValueObject
{
    public string Value { get; init; }
    private static readonly Regex valuePattern = new(@"^[a-zA-Z0-9\s.-]{1,30}$");

    [Newtonsoft.Json.JsonConstructor]
    [System.Text.Json.Serialization.JsonConstructor]
    private CategoryName(string value) => Value = value;

    internal static Result<CategoryName> Create(string value)
    {
        value = value.Trim().Capitalize();

        return valuePattern.IsMatch(value) ?
            new CategoryName(value) :
            Result.Failure<CategoryName>(CategoryErrors.InvalidNameError);
    }
}