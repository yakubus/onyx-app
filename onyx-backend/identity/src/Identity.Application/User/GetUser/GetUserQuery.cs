using Abstractions.Messaging;
using Identity.Application.Contracts.Models;

namespace Identity.Application.User.GetUser;

public sealed record GetUserQuery() : IQuery<UserModel>;