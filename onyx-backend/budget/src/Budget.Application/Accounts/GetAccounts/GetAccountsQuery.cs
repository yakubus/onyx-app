using Abstractions.Messaging;
using Budget.Application.Accounts.Models;

namespace Budget.Application.Accounts.GetAccounts;

public sealed record GetAccountsQuery : IQuery<IEnumerable<AccountModel>>
{
}