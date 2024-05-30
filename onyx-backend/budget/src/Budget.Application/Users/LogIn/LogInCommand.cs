using Abstractions.Messaging;

namespace Budget.Application.Users.LogIn;

public sealed record LogInCommand(string Email, string Password) : ICommand<string>
{
}