using Models.DataTypes;
using System.Text.Json.Serialization;
using Budget.Application.Shared.Models;
using Budget.Domain.Subcategories;

namespace Budget.Application.Subcategories.Models;

public sealed record TargetModel
{
    public MonthDate UpToMonth { get; init; }
    public MonthDate StartedAt { get; init; }
    public MoneyModel TargetAmount { get; init; }
    public MoneyModel CollectedAmount { get; init; }
    public MoneyModel AmountAssignedEveryMonth { get; init; }
    public bool IsCompleted { get; init; }
    public bool IsActive { get; init; }

    [JsonConstructor]
    [Newtonsoft.Json.JsonConstructor]
    private TargetModel(MonthDate upToMonth, MonthDate startedAt, MoneyModel targetAmount, MoneyModel collectedAmount, MoneyModel amountAssignedEveryMonth, bool isCompleted, bool isActive)
    {
        UpToMonth = upToMonth;
        StartedAt = startedAt;
        TargetAmount = targetAmount;
        CollectedAmount = collectedAmount;
        AmountAssignedEveryMonth = amountAssignedEveryMonth;
        IsCompleted = isCompleted;
        IsActive = isActive;
    }

    internal static TargetModel FromDomainModel(Target domainModel) =>
        new(domainModel.UpToMonth,
            domainModel.StartedAt,
            MoneyModel.FromDomainModel(domainModel.TargetAmount),
            MoneyModel.FromDomainModel(domainModel.CollectedAmount),
            MoneyModel.FromDomainModel(domainModel.AmountAssignedEveryMonth),
            domainModel.IsCompleted,
            domainModel.IsActive);
}