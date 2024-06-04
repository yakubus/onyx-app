namespace Identity.Application.Contracts.Models;

public sealed record AuthorizationToken
{
    public string AccessToken { get; init; }
    public string? LongLivedToken { get; init; }

    public AuthorizationToken(string accessToken, string? longLivedToken = null)
    {
        AccessToken = accessToken;
    }
}