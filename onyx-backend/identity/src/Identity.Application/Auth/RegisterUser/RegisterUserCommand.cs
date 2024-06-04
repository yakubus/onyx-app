using Abstractions.Messaging;
using Identity.Application.Contracts.Models;

namespace Identity.Application.Auth.RegisterUser;

public sealed record RegisterUserCommand(string Email, string Username, string Password, string Currency)
    : ICommand<UserModel>;