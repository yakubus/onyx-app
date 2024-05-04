using Abstractions.Messaging;
using Budget.Application.Accounts.Models;
using Budget.Application.Shared.Models;

namespace Budget.Application.Accounts.AddAccount;

public sealed record AddAccountCommand(string Name, MoneyModel Balance, string AccountType) : ICommand<AccountModel>
{
}