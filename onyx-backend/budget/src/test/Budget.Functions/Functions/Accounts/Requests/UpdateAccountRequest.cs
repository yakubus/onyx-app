using Budget.Application.Shared.Models;

namespace Budget.Functions.Functions.Accounts.Requests;

public sealed record UpdateAccountRequest(string? NewName, MoneyModel? NewBalance);