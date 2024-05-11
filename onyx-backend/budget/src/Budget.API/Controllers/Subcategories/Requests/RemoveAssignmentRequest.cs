using Models.DataTypes;

namespace Budget.API.Controllers.Subcategories.Requests;

internal sealed record RemoveAssignmentRequest
{
    public MonthDate AssignmentMonth { get; set; }
}