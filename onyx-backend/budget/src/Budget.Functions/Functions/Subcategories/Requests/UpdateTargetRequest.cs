using Models.DataTypes;

namespace Budget.Functions.Functions.Subcategories.Requests;

public sealed record UpdateTargetRequest(MonthDate StartedAt, MonthDate TargetUpToMonth, decimal TargetAmount);