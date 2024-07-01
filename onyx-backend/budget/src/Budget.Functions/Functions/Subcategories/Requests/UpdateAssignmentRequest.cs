using Models.DataTypes;

namespace Budget.Functions.Functions.Subcategories.Requests;

public sealed record UpdateAssignmentRequest(decimal AssignedAmount, MonthDate AssignmentMonth);