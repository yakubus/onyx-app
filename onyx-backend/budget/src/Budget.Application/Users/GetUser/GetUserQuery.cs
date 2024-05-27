using Abstractions.Messaging;
using Budget.Application.Users.Models;

namespace Budget.Application.Users.GetUser;

public sealed record GetUserQuery() : IQuery<UserModel>
{
}