using System.Text.RegularExpressions;
using Abstractions.DomainBaseTypes;
using Extensions.Formatters;
using Models.Responses;

namespace Budget.Domain.Subcategories;

public sealed record SubcategoryName : ValueObject
{
    public string Value { get; init; }
    private static readonly Regex valuePattern = new(@"^[a-zA-Z0-9\s.-]{1,50}$");

    private SubcategoryName(string value) => Value = value;

    public static Result<SubcategoryName> Create(string value)
    {
        value = value.Trim().Capitalize();

        return valuePattern.IsMatch(value) ?
            new SubcategoryName(value) :
            Result.Failure<SubcategoryName>(SubcategoryErrors.InvalidNameError);
    }
}