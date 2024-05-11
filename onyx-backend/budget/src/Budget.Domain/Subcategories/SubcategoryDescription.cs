using Abstractions.DomainBaseTypes;
using Models.Responses;

namespace Budget.Domain.Subcategories;

public sealed record SubcategoryDescription : ValueObject
{
    public string Value { get; init; }
    private static readonly int valueMaxLength = 255;

    [Newtonsoft.Json.JsonConstructor]
    [System.Text.Json.Serialization.JsonConstructor]
    private SubcategoryDescription(string value) => Value = value;

    internal static Result<SubcategoryDescription> Create(string value)
    {
        value = value.Trim();

        return valueMaxLength < value.Length ?
            new SubcategoryDescription(value) :
            Result.Failure<SubcategoryDescription>(SubcategoryErrors.DescriptionTooLong);
    }
}