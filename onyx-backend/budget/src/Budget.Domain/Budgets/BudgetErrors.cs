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
    internal static readonly Error UserAlreadyAdded = new (
        "Budget.Users.AlreadyAdded",
        "User is already a member of the budget");
    internal static readonly Error UserNotAdded = new (
        "Budget.Users.NotAdded",
        "User is not a member of the budget");
    internal static readonly Error TokenExpired = new (
        "Budget.InvitationToken.Expired",
        "Invitation token has expired");
    internal static readonly Error InvalidToken = new (
        "Budget.InvitationToken.Invalid",
        "Invitation token is invalid");
    internal static readonly Error InvitationTokenNotGenerated = new(
        "Budget.InvitationToken.NotGenerated",
        "Invitation token has not been generated");
}