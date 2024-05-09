using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Responses;

namespace Budget.Application.Transactions.GetTransactions
{
    internal static class GetTransactionErrors
    {
        internal static readonly Error InvalidQueryValues = new(
            "GetTransaction.InvalidQueryValues",
            "Invalid values for query");
    }
}
