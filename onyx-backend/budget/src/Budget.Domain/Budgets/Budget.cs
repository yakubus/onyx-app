using Abstractions.DomainBaseTypes;
using Models.DataTypes;
using Models.Responses;

namespace Budget.Domain.Budgets;

public sealed class Budget : Entity<BudgetId>
{
    public BudgetName Name { get; private set; }
    public Currency BaseCurrency { get; private set; }
    private readonly List<string> _userIds;
    public IReadOnlyCollection<string> UserIds => _userIds.AsReadOnly();
    private const int maxUsers = 10;
    public int MaxAccounts => 8 + 2 * (_userIds.Count - 1);
    public int MaxCategories => 15 + 5 * (_userIds.Count - 1);

    [Newtonsoft.Json.JsonConstructor]
    [System.Text.Json.Serialization.JsonConstructor]
    private Budget(
        BudgetName name,
        Currency baseCurrency,
        List<string> userIds,
        BudgetId? id = null) : base(id ?? new BudgetId())
    {
        Name = name;
        BaseCurrency = baseCurrency;
        _userIds = userIds;
    }

    public static Result<Budget> Create(string budgetName, string userId, string currencyCode)
    {
        var budgetNameCreateResult = BudgetName.Create(budgetName);

        if (budgetNameCreateResult.IsFailure)
        {
            return budgetNameCreateResult.Error;
        }

        var currencyCreateResult = Currency.FromCode(currencyCode);

        if (currencyCreateResult.IsFailure)
        {
            return currencyCreateResult.Error;
        }

        return new Budget(budgetNameCreateResult.Value, currencyCreateResult.Value, [userId]);
    }

    public Result AddUser(string userId)
    {
        if (_userIds.Count >= maxUsers)
        {
            return Result.Failure(BudgetErrors.MaxUserNumberReached);
        }

        if (_userIds.Any(id => id == userId))
        {
            return BudgetErrors.UserAlreadyAdded;
        }

        _userIds.Add(userId);

        return Result.Success();
    }

    public Result ExcludeUser(string userId)
    {
        if (_userIds.Count == 1)
        {
            return BudgetErrors.UserRemoveError;
        }

        var isFound = _userIds.Remove(userId);

        if (!isFound)
        {
            return BudgetErrors.UserNotAdded;
        }

        return Result.Success();
    }
}