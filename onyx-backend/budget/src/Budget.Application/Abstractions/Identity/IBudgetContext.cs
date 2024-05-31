using Models.Responses;

namespace Budget.Application.Abstractions.Identity;

public interface IBudgetContext
{
    Result<Guid> GetBudgetId();
}