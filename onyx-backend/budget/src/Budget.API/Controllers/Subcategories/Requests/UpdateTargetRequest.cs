using Budget.Application.Shared.Models;
using Models.DataTypes;

namespace Budget.API.Controllers.Subcategories.Requests;

public sealed record UpdateTargetRequest
{
    public MonthDate TargetUpToMonth { get; set; }
    public decimal TargetAmount { get; set; }
}