using Models.Responses;

namespace Budget.Domain.Transactions;

internal static class TransactionErrors
{
    internal static readonly Error AccountAndAmountCurrenciesDoesNotMatch = new(
        "Transaction.Amount.AccountAndAmountCurrenciesDoesNotMatch",
        "Transaction currency must be converted to the same currency as the account currency");

    internal static readonly Error TransactionIsNotForeign = new(
        "Transaction.IsNotForeign",
        "Transaction is not foreign for account");
}