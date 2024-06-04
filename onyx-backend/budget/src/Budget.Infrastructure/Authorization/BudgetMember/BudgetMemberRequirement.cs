using Microsoft.AspNetCore.Authorization;

namespace Budget.Infrastructure.Authorization.BudgetMember;

internal class BudgetMemberRequirement : IAuthorizationRequirement
{
    public static readonly string PolicyName = nameof(BudgetMemberRequirement);
}