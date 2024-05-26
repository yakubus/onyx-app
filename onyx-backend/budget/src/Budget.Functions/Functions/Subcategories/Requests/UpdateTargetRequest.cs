using Models.DataTypes;

namespace Budget.Functions.Functions.Subcategories.Requests;

public sealed record UpdateTargetRequest
{
    public MonthDate TargetUpToMonth { get; set; }
    public decimal TargetAmount { get; set; }
}