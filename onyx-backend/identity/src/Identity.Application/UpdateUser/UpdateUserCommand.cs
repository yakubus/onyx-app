using Abstractions.Messaging;
using Identity.Application.Models;

namespace Identity.Application.UpdateUser;

public sealed record UpdateUserCommand(
    Guid UserId,
    string? NewEmail,
    string? NewUsername,
    string? NewPassword,
    string? NewCurrency,
    string? VerificationCode) : ICommand<UserModel>;