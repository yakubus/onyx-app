using Budget.Application.Shared.Models;

namespace Budget.Functions.Functions.Accounts.Requests;

public sealed record AddAccountRequest(string Name, MoneyModel Balance, string AccountType);