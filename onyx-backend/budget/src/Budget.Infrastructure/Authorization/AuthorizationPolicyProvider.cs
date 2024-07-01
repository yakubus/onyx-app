using Microsoft.AspNetCore.Authorization;
using Budget.Infrastructure.Authorization.BudgetMember;

namespace Budget.Infrastructure.Authorization;

internal sealed class AuthorizationPolicyProvider
{
    public static Action<AuthorizationOptions> Configure => AddBudgetMemberPolicy;

    private static void AddBudgetMemberPolicy(AuthorizationOptions config)
    {
        config.AddPolicy(
            BudgetMemberRequirement.PolicyName,
            builder => builder
                .AddRequirements(new BudgetMemberRequirement())
                .Build());
    }
}