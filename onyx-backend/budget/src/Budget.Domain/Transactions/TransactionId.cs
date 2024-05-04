using Abstractions.DomainBaseTypes;

namespace Budget.Domain.Transactions;

public sealed record TransactionId : EntityId
{
    public TransactionId()
    { }

    public TransactionId(string value)
        : base(value)
    { }

    public TransactionId(Guid value)
        : base(value)
    { }
}