using Abstractions.Messaging;

namespace Identity.Application.RequestEmailChange;

public sealed record RequestEmailChangeCommand(string Token) : ICommand;