namespace Identity.Infrastructure.Authentication;

public sealed class AuthenticationOptions
{
    public string Audience { get; init; } = string.Empty;

    public int ExpireInMinutes { get; init; } = 60; // One hour
    public int ExpireInLongMinutes { get; init; } = 10_080; // One week

    public string SecretKey { get; set; } = string.Empty;

    public string Issuer { get; set; } = string.Empty;
}