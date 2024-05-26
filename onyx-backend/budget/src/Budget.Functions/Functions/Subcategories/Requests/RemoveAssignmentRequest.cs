using Models.DataTypes;

namespace Budget.Functions.Functions.Subcategories.Requests;

public sealed record RemoveAssignmentRequest
{
    public MonthDate AssignmentMonth { get; set; }
}