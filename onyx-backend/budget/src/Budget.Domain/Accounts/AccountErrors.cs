using Models.Responses;

namespace Budget.Domain.Accounts;

internal static class AccountErrors
{
    internal static readonly Error InvalidNameError = new(
        "AccountId.Name.InvalidValue",
        "Invalid account name");
    internal static readonly Error InconsistentCurrency = new (
        "AccountId.Currency.Inconsistent",
        "Inconsistent currency");
}