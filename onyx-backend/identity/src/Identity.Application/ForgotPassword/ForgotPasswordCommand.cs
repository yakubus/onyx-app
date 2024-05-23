using Abstractions.Messaging;

namespace Identity.Application.ForgotPassword;

public sealed record ForgotPasswordCommand(Guid UserId) : ICommand;