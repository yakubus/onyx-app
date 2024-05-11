using Budget.Application.Shared.Models;

namespace Budget.API.Controllers.Accounts.Requests;

internal sealed record UpdateAccountRequest
{
    public string? NewName { get; set; }
    public MoneyModel? NewBalance { get; set; }
}