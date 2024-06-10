namespace Budget.API.Controllers.Budgets.Requests;

public sealed record AddBudgetRequest(string BudgetName, string BudgetCurrency);