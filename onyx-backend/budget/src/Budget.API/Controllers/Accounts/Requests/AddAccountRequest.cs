using Budget.Application.Shared.Models;

namespace Budget.API.Controllers.Accounts.Requests;

public sealed record AddAccountRequest(string Name, MoneyModel Balance, string AccountType)
{
}