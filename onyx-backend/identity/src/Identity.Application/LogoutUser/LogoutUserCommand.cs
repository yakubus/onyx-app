using Abstractions.Messaging;

namespace Identity.Application.LogoutUser;

public sealed record LogoutUserCommand(Guid UserId) : ICommand;