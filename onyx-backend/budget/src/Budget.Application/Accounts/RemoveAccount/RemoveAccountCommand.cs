using Abstractions.Messaging;

namespace Budget.Application.Accounts.RemoveAccount;

public sealed record RemoveAccountCommand(Guid Id) : ICommand
{
}