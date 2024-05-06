using Abstractions.DomainBaseTypes;
using Models.Responses;

namespace Budget.Domain.Accounts;

public sealed record AccountType : ValueObject
{
    public string Value { get; init; }

    private AccountType(string value) => Value = value;

    public static readonly AccountType Checking = new("Checking");
    public static readonly AccountType Savings = new("Savings");
    public static readonly IReadOnlyCollection<AccountType> All = new List<AccountType>
    {
        Checking,
        Savings
    };

    private static readonly Error invalidAccountTypeError = new(
        "Type.InvalidValue",
        "Invalid account type");

    public static Result<AccountType> Create(string value) =>
        All.FirstOrDefault(
            type => string.Equals(
                type.Value,
                value,
                StringComparison.CurrentCultureIgnoreCase)) ??
        Result.Failure<AccountType>(invalidAccountTypeError);
}