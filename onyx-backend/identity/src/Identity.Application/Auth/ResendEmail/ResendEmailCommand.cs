using Abstractions.Messaging;

namespace Identity.Application.Auth.ResendEmail;

public sealed record ResendEmailCommand(string Email, string MessageType) : ICommand;