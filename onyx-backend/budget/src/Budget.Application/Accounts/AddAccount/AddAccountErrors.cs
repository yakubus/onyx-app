using Models.Responses;

namespace Budget.Application.Accounts.AddAccount;

internal static class AddAccountErrors
{
    internal static readonly Error AccountAlreadyExists = new (
        "AccountId.AlreadyExists",
        "AccountId already exists");
}