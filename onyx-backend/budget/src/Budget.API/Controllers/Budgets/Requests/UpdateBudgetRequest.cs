namespace Budget.API.Controllers.Budgets.Requests;

public sealed record UpdateBudgetRequest(string? UserIdToAdd, string? UserIdToRemove);