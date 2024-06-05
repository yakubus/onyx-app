using Abstractions.Messaging;
using Budget.Application.Abstractions.Currency;
using Budget.Application.Abstractions.Identity;
using Budget.Application.Shared.Models;
using Budget.Domain.Accounts;
using Budget.Domain.Budgets;
using Budget.Domain.Subcategories;
using Models.DataTypes;
using Models.Responses;

namespace Budget.Application.Subcategories.GetToAssign;

internal sealed class GetToAssignQueryHandler : IQueryHandler<GetToAssignQuery, MoneyModel>
{
    private readonly ISubcategoryRepository _subcategoryRepository;
    private readonly IAccountRepository _accountRepository;
    private readonly ICurrencyConverter _currencyConverter;
    private readonly IBudgetContext _budgetContext;
    private readonly IBudgetRepository _budgetRepository;

    public GetToAssignQueryHandler(
        IAccountRepository accountRepository,
        ICurrencyConverter currencyConverter,
        IBudgetContext budgetContext,
        IBudgetRepository budgetRepository,
        ISubcategoryRepository subcategoryRepository)
    {
        _accountRepository = accountRepository;
        _currencyConverter = currencyConverter;
        _budgetContext = budgetContext;
        _budgetRepository = budgetRepository;
        _subcategoryRepository = subcategoryRepository;
    }

    public async Task<Result<MoneyModel>> Handle(GetToAssignQuery request, CancellationToken cancellationToken)
    {
        var (forMonthCreateResult, getBudgetIdResult) = (
            MonthDate.Create(request.Month, request.Year), 
            _budgetContext.GetBudgetId()
            );

        if (Result.Aggregate([forMonthCreateResult, getBudgetIdResult]) 
                is var createResult && createResult.IsFailure)
        {
            return createResult.Error;
        }

        var (forMonth, budgetId) = (forMonthCreateResult.Value, new BudgetId(getBudgetIdResult.Value));
        
        var (accountsGetTask, subcategoriesGetTask) = (
            _accountRepository.GetAllAsync(cancellationToken),
            _subcategoryRepository.GetAllAsync(cancellationToken)
            );

        await Task.WhenAll(accountsGetTask, subcategoriesGetTask);

        var (accountsGetResult, subcategoriesGetResult) = (accountsGetTask.Result, subcategoriesGetTask.Result);

        if (Result.Aggregate([accountsGetResult, subcategoriesGetResult]) 
                is var getResult && getResult.IsFailure)
        {
            return getResult.Error;
        }

        var (accounts, subcategories) = (accountsGetResult.Value, subcategoriesGetResult.Value);

        var budgetGetResult = await _budgetRepository.GetByIdAsync(budgetId, cancellationToken);

        if (budgetGetResult.IsFailure)
        {
            return budgetGetResult.Error;
        }

        var budgetCurrency = budgetGetResult.Value.BaseCurrency;

        var overallBalanceGetResultTasks = accounts.Select(
            async a => a.Balance.Currency == budgetCurrency ?
                Result.Success(a.Balance) :
                await _currencyConverter.ConvertMoney(a.Balance, budgetCurrency, cancellationToken));

        var overallBalanceGetResult = await Task.WhenAll(overallBalanceGetResultTasks);

        if (Result.Aggregate(overallBalanceGetResult) is var result && result.IsFailure)
        {
            return result.Error;
        }

        var overallBalance = new Money(overallBalanceGetResult.Select(r => r.Value).Sum(m => m.Amount), budgetCurrency);

        var overallAssignedAmount = new Money(
            subcategories.SelectMany(s => s.Assignments)
                .Where(a => a.Month == forMonth)
                .Select(a => a.AssignedAmount)
                .Sum(m => m.Amount),
            budgetCurrency);

        var toAssign = overallBalance - overallAssignedAmount;

        return MoneyModel.FromDomainModel(toAssign);
    }
}