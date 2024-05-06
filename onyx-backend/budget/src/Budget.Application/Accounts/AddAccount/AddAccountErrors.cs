using Models.Responses;

namespace Budget.Application.Accounts.AddAccount;

internal static class AddCounterpartyErrors
{
    internal static readonly Error AccountAlreadyExists = new (
        "Account.AlreadyExists",
        "Account already exists");
}