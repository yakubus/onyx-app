using Abstractions.Messaging;
using Identity.Application.Models;

namespace Identity.Application.GetUser;

public sealed record GetUserQuery(Guid? UserId, string? Username, string? Email)
    : IQuery<UserModel>
{
}