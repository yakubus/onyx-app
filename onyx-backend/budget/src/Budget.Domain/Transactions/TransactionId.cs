using Abstractions.DomainBaseTypes;
using Budget.Domain.Converters.EntityIdConverters;
using Newtonsoft.Json;

namespace Budget.Domain.Transactions;

[JsonConverter(typeof(TransactionIdConverter))]
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