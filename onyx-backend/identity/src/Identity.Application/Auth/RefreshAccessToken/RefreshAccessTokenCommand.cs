using Abstractions.Messaging;
using Identity.Application.Contracts.Models;

namespace Identity.Application.Auth.RefreshAccessToken;

//TODO consider expired token vs user id
public sealed record RefreshAccessTokenCommand(string LongLivedToken, string ExpiredToken)
    : IQuery<AuthorizationToken>;