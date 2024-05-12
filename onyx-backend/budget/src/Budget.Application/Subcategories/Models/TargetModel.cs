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

    [JsonConstructor]
    private TargetModel(MonthDate upToMonth, MonthDate startedAt, MoneyModel targetAmount, MoneyModel collectedAmount, MoneyModel amountAssignedEveryMonth)
    {
        UpToMonth = upToMonth;
        StartedAt = startedAt;
        TargetAmount = targetAmount;
        CollectedAmount = collectedAmount;
        AmountAssignedEveryMonth = amountAssignedEveryMonth;
    }

    internal static TargetModel FromDomainModel(Target domainModel) =>
        new(domainModel.UpToMonth,
            domainModel.StartedAt,
            MoneyModel.FromDomainModel(domainModel.TargetAmount),
            MoneyModel.FromDomainModel(domainModel.CollectedAmount),
            MoneyModel.FromDomainModel(domainModel.AmountAssignedEveryMonth));
}