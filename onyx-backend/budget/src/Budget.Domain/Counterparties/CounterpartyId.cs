using Abstractions.DomainBaseTypes;

namespace Budget.Domain.Counterparties;

public sealed record CounterpartyId : EntityId
{
    public CounterpartyId() : base()
    {
        
    }

    public CounterpartyId(Guid value) : base(value)
    {
        
    }

    public CounterpartyId(string value) : base(value)
    {
        
    }
}