namespace Identity.Application.Models;

public sealed record AuthorizationToken
{
    public string AccessToken { get; init; }

    public AuthorizationToken(string accessToken)
    {
        AccessToken = accessToken;
    }
}