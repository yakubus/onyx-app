using Models.DataTypes;

namespace Budget.API.Controllers.Subcategories.Requests;

public sealed record RemoveAssignmentRequest(MonthDate AssignmentMonth);