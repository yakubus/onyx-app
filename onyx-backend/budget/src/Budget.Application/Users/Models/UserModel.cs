using Abstractions.Messaging;
using Budget.Domain.Users;
using Newtonsoft.Json;

namespace Budget.Application.Users.Models;

public sealed record UserModel : EntityBusinessModel
{
    public Guid Id { get; init; }
    public string Username { get; init; }
    public string Email { get; init; }
    public string Currency { get; init; }
    public List<Guid> BudgetIds { get; init; }

    [JsonConstructor]
    [System.Text.Json.Serialization.JsonConstructor]
    private UserModel(
        Guid id,
        string username,
        string email,
        string currency,
        List<Guid> budgetIds,
        IEnumerable<IDomainEvent> domainEvents)
        : base(domainEvents)
    {
        Id = id;
        Username = username;
        Email = email;
        BudgetIds = budgetIds;
        Currency = currency;
    }

    internal static UserModel FromDomainModel(User domainModel) =>
        new(domainModel.Id.Value,
            domainModel.Username,
            domainModel.Email,
            domainModel.Currency.Code,
            domainModel.BudgetIds.Select(x => x.Value).ToList(),
            domainModel.GetDomainEvents());
}