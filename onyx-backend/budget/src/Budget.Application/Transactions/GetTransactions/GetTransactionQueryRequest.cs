using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Budget.Domain.Subcategories;
using Models.Responses;

namespace Budget.Application.Transactions.GetTransactions
{
    internal sealed record GetTransactionQueryRequest
    {
        public string Value { get; init; }

        private GetTransactionQueryRequest(string value) => Value = value;

        private static readonly Error invalidQueryError = new (
            "GetTransactionQueryRequest.InvalidQuery",
            "Invalid query");

        internal static readonly GetTransactionQueryRequest Empty = new(string.Empty);
        internal static readonly GetTransactionQueryRequest All = new(nameof(All));
        internal static readonly GetTransactionQueryRequest Counterparty = new(nameof(Counterparty));
        internal static readonly GetTransactionQueryRequest Account = new(nameof(Account));
        internal static readonly GetTransactionQueryRequest Subcategory = new(nameof(Subcategory));
        internal static readonly GetTransactionQueryRequest Assignment = new(nameof(Assignment));

        internal static readonly IReadOnlyCollection<GetTransactionQueryRequest> AllValues =
            new List<GetTransactionQueryRequest>
            {
                Empty,
                All,
                Counterparty,
                Account,
                Subcategory,
                Assignment
            };

        internal static Result<GetTransactionQueryRequest> FromString(string value) =>
            AllValues.FirstOrDefault(
                q => string.Equals(
                    q.Value,
                    value,
                    StringComparison.CurrentCultureIgnoreCase)) ??
            Result.Failure<GetTransactionQueryRequest>(invalidQueryError);
    }
}
