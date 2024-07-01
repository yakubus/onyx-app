using Abstractions.Messaging;
using Identity.Application.Contracts.Models;

namespace Identity.Application.Auth.LoginUser;

public sealed record LoginUserCommand(string Email, string Password)
    : ICommand<AuthorizationToken>;