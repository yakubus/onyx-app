using Abstractions.Messaging;
using Budget.Application.Budgets.Models;

namespace Budget.Application.Budgets.GetBudgets;

public sealed class GetBudgetsQuery : IQuery<IEnumerable<BudgetModel>>
{
}