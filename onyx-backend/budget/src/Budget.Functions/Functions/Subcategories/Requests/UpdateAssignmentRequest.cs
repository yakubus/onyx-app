using Models.DataTypes;

namespace Budget.Functions.Functions.Subcategories.Requests;

public sealed record UpdateAssignmentRequest
{
    public decimal AssignedAmount { get; set; }
    public MonthDate AssignmentMonth { get; set; }
}