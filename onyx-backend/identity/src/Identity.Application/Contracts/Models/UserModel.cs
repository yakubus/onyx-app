using Newtonsoft.Json;

namespace Identity.Application.Contracts.Models;

public sealed record UserModel
{
    public Guid Id { get; init; }
    public string Email { get; init; }
    public string Username { get; init; }
    public string Currency { get; init; }
    public AuthorizationToken? AuthorizationToken { get; init; }

    [JsonConstructor]
    [System.Text.Json.Serialization.JsonConstructor]
    private UserModel(Guid id, string email, string username, string currency, AuthorizationToken? authorizationToken)
    {
        Id = id;
        Email = email;
        Username = username;
        Currency = currency;
        AuthorizationToken = authorizationToken;
    }

    public static UserModel FromDomainModel(Domain.User domainModel, AuthorizationToken? authorizationToken = null) =>
        new(
            domainModel.Id.Value,
            domainModel.Email.Value,
            domainModel.Username.Value,
            domainModel.Currency.Code,
            authorizationToken);
}