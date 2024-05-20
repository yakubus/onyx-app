using Budget.Application.Shared.Models;
using Models.DataTypes;

namespace Budget.API.Controllers.Subcategories.Requests;

public sealed record UpdateAssignmentRequest
{
    public decimal AssignedAmount { get; set; }
    public MonthDate AssignmentMonth { get; set; }
}