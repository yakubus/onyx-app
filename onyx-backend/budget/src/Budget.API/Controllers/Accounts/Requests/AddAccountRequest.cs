using Budget.Application.Shared.Models;

namespace Budget.API.Controllers.Accounts.Requests;

public sealed record AddAccountRequest
{
    public string Name { get; set; }
    public MoneyModel Balance { get; set; }
    public string AccountType { get; set; }
}