using Abstractions.DomainBaseTypes;
using Models.DataTypes;
using Models.Responses;

namespace Budget.Domain.Budgets;

public sealed class Budget : Entity<BudgetId>
{
    public BudgetName Name { get; private set; }
    public Currency BaseCurrency { get; private set; }
    private readonly List<string> _userIds;
    public IReadOnlyCollection<string> UserIdsReadOnly => _userIds.AsReadOnly();
    private const int maxUsers = 10;

    [Newtonsoft.Json.JsonConstructor]
    [System.Text.Json.Serialization.JsonConstructor]
    private Budget(BudgetName name, Currency baseCurrency, List<string> userIds) 
        : base()
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

        var baseCurrency = currencyCreateResult.Value;

        return new Budget(budgetNameCreateResult.Value, baseCurrency, [userId]);
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

    public Result ChangeBaseCurrency(string currencyCode)
    {
        var currencyCreateResult = Currency.FromCode(currencyCode);

        if (currencyCreateResult.IsFailure)
        {
            return currencyCreateResult.Error;
        }

        BaseCurrency = currencyCreateResult.Value;

        return Result.Success();
    }
}