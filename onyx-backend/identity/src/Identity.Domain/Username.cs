using System.Text.RegularExpressions;
using Models.Responses;

namespace Identity.Domain;

public sealed record Username
{
    public string Value { get; init; }
    private const string usernamePattern = @"^(?=.{1,30}$)[^\s][\p{L}\d\s]*[^\s]$";

    private Username(string value)
    {
        Value = value;
    }

    public static Result<Username> Create(string value) =>
        Regex.IsMatch(value, usernamePattern) ?
            new Username(value) :
            Result.Failure<Username>(UserErrors.InvalidUsername);
}