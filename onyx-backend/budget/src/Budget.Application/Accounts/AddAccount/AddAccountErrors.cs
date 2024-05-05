using Models.Responses;

namespace Budget.Application.Accounts.AddAccount;

internal static class AddAccountErrors
{
    internal static readonly Error NotSupportedAccountType = new (
        "AccountType.NotSupported",
        "Account type is not supported");
    internal static readonly Error AccountAlreadyExists = new (
        "Account.AlreadyExists",
        "Account already exists");
}