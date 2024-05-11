using Budget.Application.Shared.Models;
using Models.DataTypes;

namespace Budget.API.Controllers.Subcategories.Requests;

internal sealed record UpdateTargetRequest
{
    public MonthDate TargetUpToMonth { get; set; }
    public MoneyModel TargetAmount { get; set; }
}