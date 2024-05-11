using Abstractions.DomainBaseTypes;
using Budget.Domain.Converters.EntityIdConverters;
using Newtonsoft.Json;

namespace Budget.Domain.Counterparties;

[JsonConverter(typeof(CounterpartyIdConverter))]
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