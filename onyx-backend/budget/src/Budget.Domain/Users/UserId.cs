using Abstractions.DomainBaseTypes;

namespace Budget.Domain.Users;

public record UserId : EntityId
{
    public UserId()
    { }

    public UserId(string value) : base(value)
    { }

    public UserId(Guid value) : base(value)
    { }
}