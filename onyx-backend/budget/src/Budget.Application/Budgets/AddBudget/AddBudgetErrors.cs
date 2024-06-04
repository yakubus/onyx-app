using Models.Responses;

namespace Budget.Application.Budgets.AddBudget;

internal static class AddBudgetErrors
{
    public static readonly Error BudgetNameNotUnique = new Error(
        "AddBudget.BudgetNameNotUnique", 
        "Budget with this name already exists");
}