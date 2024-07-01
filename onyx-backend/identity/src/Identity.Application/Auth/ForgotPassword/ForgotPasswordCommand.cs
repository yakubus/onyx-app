using Abstractions.Messaging;

namespace Identity.Application.Auth.ForgotPassword;

public sealed record ForgotPasswordCommand(string Email) : ICommand;