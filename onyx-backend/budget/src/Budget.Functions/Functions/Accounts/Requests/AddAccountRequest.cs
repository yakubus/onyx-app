using Budget.Application.Shared.Models;

namespace Budget.Functions.Functions.Accounts.Requests;

public sealed record AddAccountRequest
{
    public string Name { get; set; }
    public MoneyModel Balance { get; set; }
    public string AccountType { get; set; }
}