using Budget.Application.Shared.Models;

namespace Budget.Functions.Functions.Accounts.Requests;

public sealed record UpdateAccountRequest
{
    public string? NewName { get; set; }
    public MoneyModel? NewBalance { get; set; }
}