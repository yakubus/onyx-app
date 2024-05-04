using System.Text.Json.Serialization;
using Budget.Application.Shared.Models;
using Budget.Domain.Accounts;

namespace Budget.Application.Accounts.Models;

public sealed record AccountModel
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public MoneyModel Balance { get; init; }

    [JsonConstructor]
    private AccountModel(Guid id, string name, MoneyModel balance)
    {
        Id = id;
        Name = name;
        Balance = balance;
    }

    internal static AccountModel FromDomainModel(Account domainModel) =>
        new(domainModel.Id.Value,
            domainModel.Name.Value,
            MoneyModel.FromDomainModel(domainModel.Balance));
}