using System.Text.RegularExpressions;
using Budget.Application.Abstractions.Identity;
using Microsoft.AspNetCore.Http;
using Models.Responses;

namespace Budget.Infrastructure.Contexts;

internal sealed class BudgetContext : IBudgetContext
{
    private static readonly IReadOnlyCollection<Regex> budgetIdPathRegexes =
    [
        new(@"^/api/v1/(?<budgetId>[^/]+)(/.*)?$", RegexOptions.Compiled),
        new(@"^/api/v1/budgets/(?<budgetId>[^/]+)(/.*)?$", RegexOptions.Compiled),
    ];
    private readonly IHttpContextAccessor _httpContextAccessor;

    public BudgetContext(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Result<Guid> GetBudgetId()
    {
        var path = _httpContextAccessor
            .HttpContext?
            .Request
            .Path
            .Value;

        var budgetId = string.IsNullOrWhiteSpace(path) ?
            null :
            MatchPath(path);

        return budgetId is null ?
            new Error(
                "BudgetContext.BudgetIdNotFound",
                "Cannot retrieve budget for user") :
            budgetId;
    }

    private static Guid? MatchPath(string path)
    {
        foreach (var regex in budgetIdPathRegexes)
        {
            var isValid = regex.Match(path) is var match && match.Success;

            if (!isValid)
            {
                continue;
            }

            var isGuid = Guid.TryParse(match.Groups["budgetId"].Value, out var budgetId);

            if (isGuid)
            {
                return budgetId;
            }
        }

        return null;
    }
}