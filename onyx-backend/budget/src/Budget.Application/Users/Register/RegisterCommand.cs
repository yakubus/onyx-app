using Abstractions.Messaging;

namespace Budget.Application.Users.Register;

public sealed record RegisterCommand(
    string Email,
    string Password,
    string Username,
    string Currency) : ICommand<string>
{ }