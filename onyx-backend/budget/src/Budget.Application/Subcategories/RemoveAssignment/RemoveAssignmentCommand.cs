using Budget.Application.Abstractions.Messaging;
using Budget.Application.Subcategories.Models;
using Models.DataTypes;

namespace Budget.Application.Subcategories.RemoveAssignment;

public sealed record RemoveAssignmentCommand(Guid SubcategoryId, MonthDate AssignmentMonth, Guid BudgetId) 
    : BudgetCommand<SubcategoryModel>(BudgetId);