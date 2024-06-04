using Abstractions.Messaging;
using Budget.Application.Budgets.Models;

namespace Budget.Application.Budgets.GetBudgetDetails;

public sealed record GetBudgetDetailsQuery(Guid BudgetId) : IQuery<BudgetModel>;