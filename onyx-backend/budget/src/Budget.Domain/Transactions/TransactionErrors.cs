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
    internal static readonly Error InvalidCounterpartyType = new(
        "Transaction.Counterparty.InvalidType",
        "Invalid counterparty type");
    internal static readonly Error CannotUpdateSubcategoryForUncategorizedTransactionError = new(
        "Transaction.Subcategory.Uncategorized.CannotUpdate",
        "Cannot update subcategory for uncategorized transaction");
    internal static readonly Error CannotUpdateCounterpartyWithDifferentType = new (
        "Transaction.Counterparty.DiffrentType.CannotUpdate",
        "Cannot update counterparty with different type");
    internal static readonly Error TransactionCannotBeInFuture = new (
        "Transaction.Date.CannotBeInFuture",
        "Transaction date cannot be in future");
}