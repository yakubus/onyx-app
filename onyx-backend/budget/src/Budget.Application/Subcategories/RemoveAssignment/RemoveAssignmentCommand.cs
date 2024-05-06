using Abstractions.Messaging;
using Models.DataTypes;

namespace Budget.Application.Subcategories.RemoveAssignment;

public sealed record RemoveAssignmentCommand(Guid SubcategoryId, MonthDate AssignmentMonth) : ICommand
{
}