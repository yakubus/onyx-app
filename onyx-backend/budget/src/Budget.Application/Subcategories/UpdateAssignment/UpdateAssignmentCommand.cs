using Abstractions.Messaging;
using Budget.Application.Abstractions.Messaging;
using Budget.Application.Subcategories.Models;
using Models.DataTypes;

namespace Budget.Application.Subcategories.UpdateAssignment;

public sealed record UpdateAssignmentCommand(
    Guid SubcategoryId,
    MonthDate AssignmentMonth,
    decimal AssignedAmount,
    Guid BudgetId) : BudgetCommand<SubcategoryModel>(BudgetId)
{
}