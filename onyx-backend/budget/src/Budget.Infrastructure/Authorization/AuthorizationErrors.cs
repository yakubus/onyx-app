using Models.Responses;

namespace Budget.Infrastructure.Authorization;

internal static class AuthorizationErrors
{
    public static Error BudgetMemberAuthorizationError = new(
        "Authorization.NotBudgetMember",
        "User is not member of the budget");
}