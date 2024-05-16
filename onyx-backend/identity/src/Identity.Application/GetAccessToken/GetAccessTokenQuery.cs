using Abstractions.Messaging;
using Identity.Application.Models;

namespace Identity.Application.GetAccessToken;

public sealed record GetAccessTokenQuery(string Token) 
    : IQuery<AuthorizationToken>
{
}