using Abstractions.Messaging;
using Budget.Application.Budgets.Models;

namespace Budget.Application.Budgets.AddBudget;

public sealed record AddBudgetCommand(string BudgetName) : ICommand<BudgetModel>;