using Identity.Domain;
using Newtonsoft.Json;

namespace Identity.Application.Models;

public sealed record UserModel
{
    public Guid Id { get; init; }
    public string Email { get; init; }
    public string Username { get; init; }
    public string Currency { get; init; }

    [JsonConstructor]
    [System.Text.Json.Serialization.JsonConstructor]
    private UserModel(Guid id, string email, string username, string currency)
    {
        Id = id;
        Email = email;
        Username = username;
        Currency = currency;
    }

    internal static UserModel FromDomainModel(User domainModel) =>
        new(
            domainModel.Id.Value,
            domainModel.Email.Value,
            domainModel.Username.Value,
            domainModel.Currency.Code);
}