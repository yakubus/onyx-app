using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.DataTypes;

namespace Budget.Domain.Transactions;

public sealed class Transaction
{
    public Money Amount { get; init; }
    public DateTime TransactedAt { get; init; }
}