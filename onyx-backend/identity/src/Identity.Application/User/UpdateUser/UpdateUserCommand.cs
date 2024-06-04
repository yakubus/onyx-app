using Abstractions.Messaging;
using Identity.Application.Contracts.Models;

namespace Identity.Application.User.UpdateUser;

public sealed record UpdateUserCommand(
    string? NewEmail,
    string? NewUsername,
    string? NewPassword,
    string? NewCurrency,
    string? VerificationCode) : ICommand<UserModel>;