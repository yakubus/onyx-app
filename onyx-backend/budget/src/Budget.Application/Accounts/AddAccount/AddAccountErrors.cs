using Models.Responses;

namespace Budget.Application.Accounts.AddAccount;

internal static class AddAccountErrors
{
    internal static readonly Error AccountAlreadyExists = new (
        "Account.AlreadyExists",
        "Account already exists");

    internal static readonly Error AccountMaxCountReached = new (
        "Account.MaxCountReached",
        "You have reached the maximum number of accounts");
}