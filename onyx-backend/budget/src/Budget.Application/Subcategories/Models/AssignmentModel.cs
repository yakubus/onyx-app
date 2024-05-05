using Models.DataTypes;
using System.Text.Json.Serialization;
using Budget.Application.Shared.Models;
using Budget.Domain.Subcategories;

namespace Budget.Application.Subcategories.Models;

public sealed record AssignmentModel
{
    public MonthDate Month { get; init; }
    public MoneyModel AssignedAmount { get; init; }
    public MoneyModel ActualAmount { get; init; }

    [JsonConstructor]
    private AssignmentModel(MonthDate month, MoneyModel assignedAmount, MoneyModel actualAmount)
    {
        Month = month;
        AssignedAmount = assignedAmount;
        ActualAmount = actualAmount;
    }

    internal static AssignmentModel FromDomainModel(Assignment assignment) =>
        new(assignment.Month,
            MoneyModel.FromDomainModel(assignment.AssignedAmount),
            MoneyModel.FromDomainModel(assignment.ActualAmount));
}