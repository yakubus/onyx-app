using Models.DataTypes;

namespace Budget.API.Controllers.Subcategories.Requests;

public sealed record RemoveAssignmentRequest
{
    public MonthDate AssignmentMonth { get; set; }

    private RemoveAssignmentRequest(MonthDate assignmentMonth)
    {
        AssignmentMonth = assignmentMonth;
    }
}