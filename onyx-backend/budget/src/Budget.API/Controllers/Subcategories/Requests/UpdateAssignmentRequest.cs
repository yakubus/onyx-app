using Budget.Application.Shared.Models;
using Models.DataTypes;

namespace Budget.API.Controllers.Subcategories.Requests;

internal sealed record UpdateAssignmentRequest
{
    public MoneyModel AssignedAmount { get; set; }
    public MonthDate AssignmentMonth { get; set; }
}