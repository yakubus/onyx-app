using Abstractions.DomainBaseTypes;
using Models.Responses;

namespace Budget.Domain.Budgets;

public sealed class Budget : Entity<BudgetId>
{
    public BudgetName Name { get; private set; }
    private readonly List<string> _userIds;
    public IReadOnlyCollection<string> UserIdsReadOnly => _userIds.AsReadOnly();
    private const int maxUsers = 10;

    [Newtonsoft.Json.JsonConstructor]
    [System.Text.Json.Serialization.JsonConstructor]
    private Budget(BudgetName name, List<string> userIds)
    {
        Name = name;
        _userIds = userIds;
    }

    public static Result<Budget> Create(string budgetName, string userId)
    {
        var budgetNameCreateResult = BudgetName.Create(budgetName);

        if (budgetNameCreateResult.IsFailure)
        {
            return Result.Failure<Budget>(budgetNameCreateResult.Error);
        }

        return new Budget(budgetNameCreateResult.Value, [userId]);
    }

    public Result AddUser(string userId)
    {
        if (_userIds.Count >= maxUsers)
        {
            return Result.Failure(BudgetErrors.MaxUserNumberReached);
        }

        _userIds.Add(userId);

        return Result.Success();
    }

    public Result ExcludeUser(string userId)
    {
        _userIds.Remove(userId);

        return Result.Success();
    }
}