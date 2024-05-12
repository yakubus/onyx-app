using Abstractions.DomainBaseTypes;
using Budget.Domain.Converters.EntityIdConverters;
using Newtonsoft.Json;

namespace Budget.Domain.Accounts;

[JsonConverter(typeof(AccountIdConverter))]
public sealed record AccountId : EntityId
{
    public AccountId() : base()
    { }

    public AccountId(string value) : base(value)
    { }

    public AccountId(Guid value) : base(value)
    { }
}