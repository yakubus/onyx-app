using Budget.Application.Abstractions.Messaging;
using Budget.Application.Accounts.Models;

namespace Budget.Application.Accounts.GetAccounts;

public sealed record GetAccountsQuery(Guid BudgetId) : BudgetQuery<IEnumerable<AccountModel>>(BudgetId)
{
}