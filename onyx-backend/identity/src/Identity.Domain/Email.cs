using System.Text.RegularExpressions;
using Models.Responses;

namespace Identity.Domain;

public sealed record Email
{
    public string Value { get; init; }
    private const string emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

    private Email(string value)
    {
        Value = value.ToLower();
    }

    public static Result<Email> Create(string value)
    {
        if (!Regex.IsMatch(value, emailPattern))
        {
            return Result.Failure<Email>(UserErrors.InvalidEmail);
        }

        return new Email(value);
    }
}