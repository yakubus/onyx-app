using Abstractions.DomainBaseTypes;
using Budget.Domain.Budgets;
using Models.DataTypes;
using Newtonsoft.Json;

namespace Budget.Domain.Users;

public sealed class User : Entity<UserId>
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string Username { get; set; }
    public Currency Currency { get; set; }
    public IEnumerable<BudgetId> BudgetIds { get; set; }

    [JsonConstructor]
    [System.Text.Json.Serialization.JsonConstructor]
    public User(
        string email,
        string password,
        string username,
        Currency currency,
        IEnumerable<BudgetId> budgetIds,
        UserId? id = null) : base(id ?? new UserId())
    {
        Email = email;
        Password = password;
        Username = username;
        Currency = currency;
        BudgetIds = budgetIds;
    }
}