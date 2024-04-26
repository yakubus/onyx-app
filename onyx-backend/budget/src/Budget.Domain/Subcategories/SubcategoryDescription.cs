using Abstractions.DomainBaseTypes;
using Models.Responses;
using System.Text.RegularExpressions;
using Extensions.Formatters;

namespace Budget.Domain.Subcategories;

public sealed record SubcategoryDescription : ValueObject
{
    public string Value { get; init; }
    private static readonly int valueMaxLength = 255;

    private SubcategoryDescription(string value) => Value = value;

    public static Result<SubcategoryDescription> Create(string value)
    {
        value = value.Trim();

        return valueMaxLength < value.Length ?
            new SubcategoryDescription(value) :
            Result.Failure<SubcategoryDescription>(SubcategoryErrors.DescriptionTooLong);
    }
}