using Abstractions.DomainBaseTypes;

namespace Identity.Domain;

public sealed record UserId : EntityId
{
    public UserId() { }

    public UserId(string value) : base(value) { }

    public UserId(Guid value) : base(value) { }
}