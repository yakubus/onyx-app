using System.Security.Claims;
using Identity.Domain;

namespace Identity.Infrastructure.Authentication.Models;

public sealed record UserRepresentationModel
{
    public static readonly string TokenCratedTimeStampClaimName = nameof(TokenCreatedTimestamp);
    public long TokenCreatedTimestamp { get; init; }

    public static readonly string EmailClaimName = nameof(Email);
    public string Email { get; init; }

    public static readonly string EmailVerifiedClaimName = nameof(EmailVerified);
    public bool EmailVerified { get; init; }

    public static readonly string IdClaimName = nameof(Id);
    public string Id { get; init; }

    public static readonly string UsernameClaimName = nameof(Username);
    public string Username { get; init; }

    public static readonly string CurrencyClaimName = nameof(Currency);
    public string Currency { get; init; }

    public static readonly string IsAuthenticatedClaimName = nameof(IsAuthenticated);
    public string IsAuthenticated { get; init; }

    private UserRepresentationModel(long tokenCreatedTimestamp, string email, bool emailVerified, string id, string username, string currency, string isAuthenticated)
    {
        TokenCreatedTimestamp = tokenCreatedTimestamp;
        Email = email;
        EmailVerified = emailVerified;
        Id = id;
        Username = username;
        Currency = currency;
        IsAuthenticated = isAuthenticated;
    }

    internal static UserRepresentationModel FromUser(User user) =>
        new(
            DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
            user.Email.Value,
            user.IsEmailVerified,
            user.Id.Value.ToString(),
            user.Email.Value,
            user.Currency.Code,
            user.IsAuthenticated.ToString());

    internal Claim[] ToClaims()
    {
        return new[]
        {
            new Claim(TokenCratedTimeStampClaimName, TokenCreatedTimestamp.ToString()),
            new Claim(EmailClaimName, Email),
            new Claim(EmailVerifiedClaimName, EmailVerified.ToString()),
            new Claim(IdClaimName, Id),
            new Claim(UsernameClaimName, Username),
            new Claim(CurrencyClaimName, Currency),
            new Claim(IsAuthenticatedClaimName, IsAuthenticated)
        };
    }
}