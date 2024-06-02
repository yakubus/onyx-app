using Models.DataTypes;

namespace Budget.API.Controllers.Subcategories.Requests;

public sealed record UpdateAssignmentRequest(decimal AssignedAmount, MonthDate AssignmentMonth);