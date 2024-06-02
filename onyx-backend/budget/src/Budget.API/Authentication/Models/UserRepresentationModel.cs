using System.Security.Claims;
using Budget.Domain.Users;

namespace Budget.API.Authentication.Models;

public sealed record UserRepresentationModel
{
    public static readonly string TokenCratedTimeStampClaimName = nameof(TokenCreatedTimestamp);
    public long TokenCreatedTimestamp { get; init; }

    public static readonly string EmailClaimName = nameof(Email);
    public string Email { get; init; }

    public static readonly string IdClaimName = nameof(Id);
    public string Id { get; init; }

    public static readonly string UsernameClaimName = nameof(Username);
    public string Username { get; init; }

    public static readonly string CurrencyClaimName = nameof(Currency);
    public string Currency { get; init; }

    private UserRepresentationModel(
        long tokenCreatedTimestamp,
        string email,
        string id,
        string username,
        string currency)
    {
        TokenCreatedTimestamp = tokenCreatedTimestamp;
        Email = email;
        Id = id;
        Username = username;
        Currency = currency;
    }

    internal static UserRepresentationModel FromUser(User user) =>
        new(
            DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
            user.Email,
            user.Id.Value.ToString(),
            user.Username,
            user.Currency.Code);

    internal Claim[] ToClaims()
    {
        return new[]
        {
            new Claim(TokenCratedTimeStampClaimName, TokenCreatedTimestamp.ToString()),
            new Claim(EmailClaimName, Email),
            new Claim(IdClaimName, Id),
            new Claim(UsernameClaimName, Username),
            new Claim(CurrencyClaimName, Currency),
        };
    }
}