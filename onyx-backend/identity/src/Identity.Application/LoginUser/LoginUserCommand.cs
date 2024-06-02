using Abstractions.Messaging;
using Identity.Application.Models;

namespace Identity.Application.LoginUser;

public sealed record LoginUserCommand(string? Email, string Password)
    : ICommand<AuthorizationToken>;