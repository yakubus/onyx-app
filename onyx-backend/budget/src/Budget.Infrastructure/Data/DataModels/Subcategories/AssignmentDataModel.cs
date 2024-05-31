using System.Reflection;
using Budget.Domain.Subcategories;
using Models.DataTypes;
using SharedDAL.DataModels.Abstractions;

namespace Budget.Infrastructure.Data.DataModels.Subcategories;

internal sealed class AssignmentDataModel : IDataModel
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

    public Assignment ToDomainModel()
    {
        var month = Activator.CreateInstance(
                        typeof(MonthDate),
                        BindingFlags.Instance | BindingFlags.NonPublic,
                        null,
                        [MonthMonth, MonthYear],
                        null) as MonthDate ??
                    throw new DataModelConversionException(
                        typeof(int),
                        typeof(MonthDate),
                        typeof(AssignmentDataModel));

        var assignedAmountCurrency = Activator.CreateInstance(
                                         typeof(Currency),
                                         BindingFlags.Instance | BindingFlags.NonPublic,
                                         null,
                                         [AssignedAmountCurrency],
                                         null) as Currency ??
                                     throw new DataModelConversionException(
                                         typeof(string),
                                         typeof(Currency),
                                         typeof(AssignmentDataModel));

        var actualAmountCurrency = Activator.CreateInstance(
                                       typeof(Currency),
                                       BindingFlags.Instance | BindingFlags.NonPublic,
                                       null,
                                       [ActualAmountCurrency],
                                       null) as Currency ??
                                   throw new DataModelConversionException(
                                       typeof(string),
                                       typeof(Currency),
                                       typeof(AssignmentDataModel));

        var assignedAmount = new Money(AssignedAmountAmount, assignedAmountCurrency);
        var actualAmount = new Money(ActualAmountAmount, actualAmountCurrency);

        return Activator.CreateInstance(
                   typeof(Assignment),
                   BindingFlags.Instance | BindingFlags.NonPublic,
                   null,
                   [month, assignedAmount, actualAmount],
                   null) as Assignment ??
               throw new DataModelConversionException(
                   typeof(AssignmentDataModel),
                   typeof(Assignment));
    }
}