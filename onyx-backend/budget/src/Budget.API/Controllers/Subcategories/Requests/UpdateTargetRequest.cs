using Models.DataTypes;

namespace Budget.API.Controllers.Subcategories.Requests;

public sealed record UpdateTargetRequest
{
    public MonthDate TargetUpToMonth { get; set; }
    public decimal TargetAmount { get; set; }

    private UpdateTargetRequest(MonthDate targetUpToMonth, decimal targetAmount)
    {
        TargetUpToMonth = targetUpToMonth;
        TargetAmount = targetAmount;
    }
}