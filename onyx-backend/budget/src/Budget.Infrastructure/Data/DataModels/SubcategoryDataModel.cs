using Budget.Domain.Subcategories;
using SharedDAL.DataModels.Abstractions;

namespace Budget.Infrastructure.Data.DataModels;

internal sealed class SubcategoryDataModel : IDataModel
{
    public Guid Id { get; set; }
    public Guid BudgetId { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public IEnumerable<AssignmentDataModel>? Assignments { get; set; }
    public int? TargetUpToMonthMonth { get; set; }
    public int? TargetUpToMonthYear { get; set; }
    public int? TargetStartedAtMonth { get; set; }
    public int? TargetStartedAtYear { get; set; }
    public decimal? TargetTargetAmount { get; set; }
    public string? TargetTargetCurrency { get; set; }
    public decimal? TargetCollectedAmount { get; set; }
    public string? TargetCollectedCurrency { get; set; }

    public static SubcategoryDataModel FromDomainModel(Subcategory subcategory)
    {
        return new SubcategoryDataModel
        {
            Id = subcategory.Id.Value,
            BudgetId = subcategory.BudgetId.Value,
            Name = subcategory.Name.Value,
            Description = subcategory.Description?.Value,
            Assignments = subcategory.Assignments.Select(AssignmentDataModel.FromDomainModel),
            TargetUpToMonthMonth = subcategory.Target?.UpToMonth?.Month,
            TargetUpToMonthYear = subcategory.Target?.UpToMonth?.Year,
            TargetStartedAtMonth = subcategory.Target?.StartedAt?.Month,
            TargetStartedAtYear = subcategory.Target?.StartedAt?.Year,
            TargetTargetAmount = subcategory.Target?.TargetAmount?.Amount,
            TargetTargetCurrency = subcategory.Target?.TargetAmount?.Currency.Code,
            TargetCollectedAmount = subcategory.Target?.CollectedAmount?.Amount,
            TargetCollectedCurrency = subcategory.Target?.CollectedAmount?.Currency.Code,
        };
    }

    public Type GetDomainModelType() => typeof(Subcategory);

    public sealed class AssignmentDataModel : IDataModel
    {
        public int MonthMonth { get; set; }
        public int MonthYear { get; set; }
        public decimal AssignedAmountAmount { get; set; }
        public string AssignedAmountCurrency { get; set; }
        public decimal ActualAmountAmount { get; set; }
        public string ActualAmountCurrency { get; set; }

        public static AssignmentDataModel FromDomainModel(Assignment assignment)
        {
            return new AssignmentDataModel
            {
                MonthMonth = assignment.Month.Month,
                MonthYear = assignment.Month.Year,
                AssignedAmountAmount = assignment.AssignedAmount.Amount,
                AssignedAmountCurrency = assignment.AssignedAmount.Currency.Code,
                ActualAmountAmount = assignment.ActualAmount.Amount,
                ActualAmountCurrency = assignment.ActualAmount.Currency.Code,
            };
        }

        public Type GetDomainModelType() => typeof(Assignment);
    }
}