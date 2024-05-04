using Models.Responses;

namespace Budget.Application.Accounts.AddAccount;

internal static class AddAccountErrors
{
    internal static readonly Error NotSupportedAccountType = new Error(
        "AccountType.NotSupported",
        "Account type is not supported");
}