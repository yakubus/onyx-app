using Abstractions.DomainBaseTypes;
using Models.Responses;

namespace Budget.Domain.Budgets;

public sealed class Budget : Entity<BudgetId>
{
    public BudgetName Name { get; private set; }
    private List<string> userIds { get; init; }
    public IReadOnlyCollection<string> UserIdsReadOnly => UserIds.AsReadOnly();
    private const int maxUsers = 10;

    [Newtonsoft.Json.JsonConstructor]
    [System.Text.Json.Serialization.JsonConstructor]
    private Budget(BudgetName name, List<string> userIds)
    {
        Name = name;
        this.userIds = userIds;
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
        if (userIds.Count >= maxUsers)
        {
            return Result.Failure(BudgetErrors.MaxUserNumberReached);
        }

        userIds.Add(userId);

        return Result.Success();
    }

    public Result ExcludeUser(string userId)
    {
        userIds.Remove(userId);

        return Result.Success();
    }
}