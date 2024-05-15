using Abstractions.Messaging;
using Identity.Application.Models;

namespace Identity.Application.GetAccessToken;

public sealed record GetAccessTokenQuery(Guid? UserId, string? Username, string? Email) 
    : IQuery<AuthorizationToken>
{
}