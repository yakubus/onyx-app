using Abstractions.DomainBaseTypes;

namespace Budget.Domain.Accounts;

public sealed record AccountId : EntityId
{
    public AccountId() : base()
    { }

    public AccountId(string value) : base(value)
    { }

    public AccountId(Guid value) : base(value)
    { }
}