using Abstractions.DomainBaseTypes;
using Budget.Domain.Converters.EntityIdConverters;
using Newtonsoft.Json;

namespace Budget.Domain.Users;

[JsonConverter(typeof(UserIdConverter))]
public record UserId : EntityId
{
    public UserId()
    { }

    public UserId(string value) : base(value)
    { }

    public UserId(Guid value) : base(value)
    { }
}