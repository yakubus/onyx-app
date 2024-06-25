using Models.Responses;

namespace Budget.Application.Transactions.GetTransactions;

public sealed record GetTransactionQueryRequest
{
    public string Value { get; init; }

    private GetTransactionQueryRequest(string value) => Value = value;

    private static readonly Error invalidQueryError = new (
        "GetTransactionQueryRequest.InvalidQuery",
        "Invalid query");

    public static readonly GetTransactionQueryRequest Empty = new(string.Empty);
    public static readonly GetTransactionQueryRequest All = new(nameof(All));
    public static readonly GetTransactionQueryRequest Counterparty = new(nameof(Counterparty));
    public static readonly GetTransactionQueryRequest Account = new(nameof(Account));
    public static readonly GetTransactionQueryRequest Subcategory = new(nameof(Subcategory));

    internal static readonly IReadOnlyCollection<GetTransactionQueryRequest> AllValues =
        new List<GetTransactionQueryRequest>
        {
            Empty,
            All,
            Counterparty,
            Account,
            Subcategory
        };

    internal static Result<GetTransactionQueryRequest> FromString(string value) =>
        AllValues.FirstOrDefault(
            q => string.Equals(
                q.Value,
                value,
                StringComparison.CurrentCultureIgnoreCase)) ??
        Result.Failure<GetTransactionQueryRequest>(invalidQueryError);
}