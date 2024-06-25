using System.Reflection;
using Amazon.DynamoDBv2.DocumentModel;
using Budget.Domain.Budgets;
using Budget.Domain.Subcategories;
using Models.DataTypes;
using SharedDAL.DataModels;
using SharedDAL.DataModels.Abstractions;
using SharedDAL.Extensions;

namespace Budget.Infrastructure.Data.DataModels.Subcategories;

internal sealed class SubcategoryDataModel : IDataModel<Subcategory>
{
    public Guid Id { get; init; }
    public Guid BudgetId { get; init; }
    public string Name { get; init; }
    public string? Description { get; init; }
    public IEnumerable<AssignmentDataModel> Assignments { get; init; }
    public int? TargetUpToMonthMonth { get; init; }
    public int? TargetUpToMonthYear { get; init; }
    public int? TargetStartedAtMonth { get; init; }
    public int? TargetStartedAtYear { get; init; }
    public decimal? TargetTargetAmount { get; init; }
    public string? TargetTargetCurrency { get; init; }
    public decimal? TargetCollectedAmount { get; init; }
    public string? TargetCollectedCurrency { get; init; }

    private SubcategoryDataModel(Document doc)
    {
        Id = doc[nameof(Id)].AsGuid();
        BudgetId = doc[nameof(BudgetId)].AsGuid();
        Name = doc[nameof(Name)];
        Description = doc[nameof(Description)].AsNullableString();
        Assignments = doc[nameof(Assignments)]
            .AsArrayOfDynamoDBEntry()
            .Select(entry => AssignmentDataModel.FromDocument(entry.AsDocument()));
        TargetUpToMonthMonth = doc[nameof(TargetUpToMonthMonth)].AsNullableInt();
        TargetUpToMonthYear = doc[nameof(TargetUpToMonthYear)].AsNullableInt();
        TargetStartedAtMonth = doc[nameof(TargetStartedAtMonth)].AsNullableInt();
        TargetStartedAtYear = doc[nameof(TargetStartedAtYear)].AsNullableInt();
        TargetTargetAmount = doc[nameof(TargetTargetAmount)].AsNullableDecimal();
        TargetTargetCurrency = doc[nameof(TargetTargetCurrency)].AsNullableString();
        TargetCollectedAmount = doc[nameof(TargetCollectedAmount)].AsNullableDecimal();
        TargetCollectedCurrency = doc[nameof(TargetCollectedCurrency)].AsNullableString();
    }

    private SubcategoryDataModel(Subcategory subcategory)
    {
        Id = subcategory.Id.Value;
        BudgetId = subcategory.BudgetId.Value;
        Name = subcategory.Name.Value;
        Description = subcategory.Description?.Value;
        Assignments = subcategory.Assignments.Select(AssignmentDataModel.FromDomainModel);
        TargetUpToMonthMonth = subcategory.Target?.UpToMonth?.Month;
        TargetUpToMonthYear = subcategory.Target?.UpToMonth?.Year;
        TargetStartedAtMonth = subcategory.Target?.StartedAt?.Month;
        TargetStartedAtYear = subcategory.Target?.StartedAt?.Year;
        TargetTargetAmount = subcategory.Target?.TargetAmount?.Amount;
        TargetTargetCurrency = subcategory.Target?.TargetAmount?.Currency.Code;
        TargetCollectedAmount = subcategory.Target?.CollectedAmount?.Amount;
        TargetCollectedCurrency = subcategory.Target?.CollectedAmount?.Currency.Code;
    }

    public static SubcategoryDataModel FromDomainModel(Subcategory subcategory) => new(subcategory);

    public Type GetDomainModelType() => typeof(Subcategory);

    public Subcategory ToDomainModel()
    {
        var id = new SubcategoryId(Id);
        var budgetId = new BudgetId(BudgetId);

        var name = Activator.CreateInstance(
                       typeof(SubcategoryName),
                       BindingFlags.Instance | BindingFlags.NonPublic,
                       null,
                       [Name],
                       null) as SubcategoryName ??
                   throw new DataModelConversionException(
                       Name,
                       typeof(SubcategoryName),
                       this);

        var description = Description is null ?
            null :
            Activator.CreateInstance(
                       typeof(SubcategoryDescription),
                       BindingFlags.Instance | BindingFlags.NonPublic,
                       null,
                       [Description],
                       null) as SubcategoryDescription ??
                   throw new DataModelConversionException(
                       Name,
                       typeof(SubcategoryDescription),
                       this);

        var assignments = Assignments.Select(_ => ToDomainModel());
        Target? target = null;

        if (TargetCollectedAmount is not null &&
            TargetCollectedCurrency is not null &&
            TargetStartedAtMonth is not null &&
            TargetStartedAtYear is not null &&
            TargetTargetAmount is not null &&
            TargetTargetCurrency is not null &&
            TargetUpToMonthMonth is not null &&
            TargetUpToMonthYear is not null)
        {
            target = CreateTargetInstance();
        }

        return Activator.CreateInstance(
                   typeof(Subcategory),
                   BindingFlags.Instance | BindingFlags.NonPublic,
                   null,
                   [
                       name,
                       description,
                       assignments,
                       target,
                       budgetId,
                       id
                   ],
                   null) as Subcategory ??
               throw new DataModelConversionException(
                   typeof(SubcategoryDataModel),
                   typeof(Subcategory));
    }

    private Target CreateTargetInstance()
    {
        var targetCollectedCurrency = Activator.CreateInstance(
                                          typeof(Currency),
                                          BindingFlags.Instance | BindingFlags.NonPublic,
                                          null,
                                          [TargetCollectedCurrency],
                                          null) as Currency ??
                                      throw new DataModelConversionException(
                                          typeof(string),
                                          typeof(Currency),
                                          typeof(SubcategoryDataModel));

        var targetTargetCurrency = Activator.CreateInstance(
                                          typeof(Currency),
                                          BindingFlags.Instance | BindingFlags.NonPublic,
                                          null,
                                          [TargetTargetCurrency],
                                          null) as Currency ??
                                      throw new DataModelConversionException(
                                          typeof(string),
                                          typeof(Currency),
                                          typeof(SubcategoryDataModel));

        var targetUpToMonth = Activator.CreateInstance(
                                  typeof(MonthDate),
                                  BindingFlags.Instance | BindingFlags.NonPublic,
                                  null,
                                  [TargetUpToMonthMonth, TargetUpToMonthYear],
                                  null) as MonthDate ??
                              throw new DataModelConversionException(
                                  typeof(int),
                                  typeof(MonthDate),
                                  typeof(SubcategoryDataModel));

        var targetStartedAt = Activator.CreateInstance(
                                  typeof(MonthDate),
                                  BindingFlags.Instance | BindingFlags.NonPublic,
                                  null,
                                  [TargetStartedAtMonth, TargetStartedAtYear],
                                  null) as MonthDate ??
                              throw new DataModelConversionException(
                                  typeof(int),
                                  typeof(MonthDate),
                                  typeof(SubcategoryDataModel));

        var collectedAmount = new Money(TargetCollectedAmount!.Value, targetCollectedCurrency);
        var targetAmount = new Money(TargetTargetAmount!.Value, targetTargetCurrency);

        return Activator.CreateInstance(
                         typeof(Target),
                         BindingFlags.Instance | BindingFlags.NonPublic,
                         null,
                         [
                             targetUpToMonth,
                             targetAmount,
                             collectedAmount,
                             targetStartedAt
                         ],
                         null) as Target ??
                     throw new DataModelConversionException(
                         typeof(object),
                         typeof(Target),
                         typeof(SubcategoryDataModel));
    }

    public static SubcategoryDataModel FromDocument(Document doc) => new(doc);
}