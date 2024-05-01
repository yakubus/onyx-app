using System.Text.RegularExpressions;
using Models.Responses;

namespace Budget.Domain.Accounts;

public sealed record AccountName
{
    public string Value { get; private set; }
    private static readonly Regex valuePattern = new(@"^[a-zA-Z0-9\s.-]{1,50}$");

    private AccountName(string value) => Value = value;

    public static Result<AccountName> Create(string value)
    {
        value = value.Trim();

        return valuePattern.IsMatch(value) ?
            new AccountName(value) :
            Result.Failure<AccountName>(AccountErrors.InvalidNameError);
    }
}