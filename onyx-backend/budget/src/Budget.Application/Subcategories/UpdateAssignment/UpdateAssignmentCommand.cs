using Abstractions.Messaging;
using Budget.Application.Shared.Models;
using Budget.Application.Subcategories.Models;
using Models.DataTypes;

namespace Budget.Application.Subcategories.UpdateAssignment;

public sealed record UpdateAssignmentCommand(
    Guid SubcategoryId,
    MonthDate Month,
    MoneyModel AssignedAmount) : ICommand<SubcategoryModel>
{
}