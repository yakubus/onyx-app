using Models.Responses;

namespace Budget.Domain.Budgets;

internal static class BudgetErrors
{
    internal static readonly Error InvalidNameError = new(
        "BudgetName.InvalidName",
        "Budget name is invalid");
    internal static readonly Error MaxUserNumberReached = new(
        "Budget.MaxUserNumberReached",
        "Maximum number of users reached");
    internal static readonly Error UserRemoveError = new (
        "Budget.Users.CannotRemove",
        "Budget must have at least one user");
}