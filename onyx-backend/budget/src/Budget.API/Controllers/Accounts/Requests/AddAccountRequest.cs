using Budget.Application.Shared.Models;

namespace Budget.API.Controllers.Accounts.Requests;

public sealed record AddAccountRequest
{
    public string Name { get; set; }
    public MoneyModel Balance { get; set; }
    public string AccountType { get; set; }

    private AddAccountRequest(string name, MoneyModel balance, string accountType)
    {
        Name = name;
        Balance = balance;
        AccountType = accountType;
    }
}