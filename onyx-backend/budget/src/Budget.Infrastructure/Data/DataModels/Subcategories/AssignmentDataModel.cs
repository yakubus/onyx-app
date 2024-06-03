using System.Reflection;
using Amazon.DynamoDBv2.DocumentModel;
using Budget.Domain.Subcategories;
using Models.DataTypes;
using SharedDAL.DataModels.Abstractions;

namespace Budget.Infrastructure.Data.DataModels.Subcategories;

internal sealed class AssignmentDataModel : IDataModel<Assignment>
{
    public int MonthMonth { get; init; }
    public int MonthYear { get; init; }
    public decimal AssignedAmountAmount { get; init; }
    public string AssignedAmountCurrency { get; init; }
    public decimal ActualAmountAmount { get; init; }
    public string ActualAmountCurrency { get; init; }

    private AssignmentDataModel(Assignment assignment)
    {
        MonthMonth = assignment.Month.Month;
        MonthYear = assignment.Month.Year;
        AssignedAmountAmount = assignment.AssignedAmount.Amount;
        AssignedAmountCurrency = assignment.AssignedAmount.Currency.Code;
        ActualAmountAmount = assignment.ActualAmount.Amount;
        ActualAmountCurrency = assignment.ActualAmount.Currency.Code;
    }

    private AssignmentDataModel(Document doc)
    {
        MonthMonth = doc[nameof(MonthMonth)].AsInt();
        MonthYear = doc[nameof(MonthYear)].AsInt();
        AssignedAmountAmount = doc[nameof(AssignedAmountAmount)].AsDecimal();
        AssignedAmountCurrency = doc[nameof(AssignedAmountCurrency)];
        ActualAmountAmount = doc[nameof(ActualAmountAmount)].AsDecimal();
        ActualAmountCurrency = doc[nameof(ActualAmountCurrency)];
    }

    public static AssignmentDataModel FromDomainModel(Assignment assignment) => new(assignment);
    public static AssignmentDataModel FromDocument(Document doc) => new(doc);

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