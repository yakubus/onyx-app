using Abstractions.Messaging;

namespace Identity.Application.User.RemoveUser;

public sealed record RemoveUserCommand(string Password) : ICommand;