using Models.Responses;

namespace Budget.Domain.Accounts;

internal static class AccountErrors
{
    internal static readonly Error InvalidNameError = new(
        "Account.Name.InvalidValue",
        "Invalid account name");
}