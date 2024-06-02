using Abstractions.Messaging;

namespace Identity.Application.RemoveUser;

public sealed record RemoveUserCommand(Guid UserId, string Password) : ICommand
{
}