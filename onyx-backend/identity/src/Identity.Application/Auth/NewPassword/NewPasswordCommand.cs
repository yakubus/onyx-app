using Abstractions.Messaging;
using Identity.Application.Contracts.Models;

namespace Identity.Application.Auth.NewPassword;

public sealed record NewPasswordCommand(string Email, string NewPassword, string VerificationCode) : ICommand<AuthorizationToken>
{
}