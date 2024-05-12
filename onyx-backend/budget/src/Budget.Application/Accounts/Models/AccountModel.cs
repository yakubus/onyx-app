using System.Text.Json.Serialization;
using Abstractions.Messaging;
using Budget.Application.Shared.Models;
using Budget.Domain.Accounts;

namespace Budget.Application.Accounts.Models;

public sealed record AccountModel : EntityBusinessModel
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public MoneyModel Balance { get; init; }
    public string Type { get; init; }

    [JsonConstructor]
    private AccountModel(Guid id, string name, MoneyModel balance, string type, IEnumerable<IDomainEvent> domainEvents)
        : base(domainEvents)
    {
        Id = id;
        Name = name;
        Balance = balance;
        Type = type;
    }

    internal static AccountModel FromDomainModel(Account domainModel) =>
        new(
            domainModel.Id.Value,
            domainModel.Name.Value,
            MoneyModel.FromDomainModel(domainModel.Balance),
            domainModel.Type.Value,
            domainModel.GetDomainEvents());
}