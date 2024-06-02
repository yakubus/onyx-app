using Models.DataTypes;

namespace Budget.API.Controllers.Subcategories.Requests;

public sealed record UpdateTargetRequest(MonthDate StartedAt, MonthDate TargetUpToMonth, decimal TargetAmount);