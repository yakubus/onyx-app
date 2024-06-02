using Budget.Application.Shared.Models;

namespace Budget.API.Controllers.Accounts.Requests;

public sealed record UpdateAccountRequest(string? NewName, MoneyModel? NewBalance);