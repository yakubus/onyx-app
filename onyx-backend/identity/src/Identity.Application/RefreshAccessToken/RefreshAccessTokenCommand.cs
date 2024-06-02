using Abstractions.Messaging;
using Identity.Application.Models;

namespace Identity.Application.RefreshAccessToken;

public sealed record RefreshAccessTokenCommand(string Token) 
    : IQuery<AuthorizationToken>;