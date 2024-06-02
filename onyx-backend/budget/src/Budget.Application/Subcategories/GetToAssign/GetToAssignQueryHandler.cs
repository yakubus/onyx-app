using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abstractions.Messaging;
using Budget.Application.Abstractions.Currency;
using Budget.Application.Abstractions.Identity;
using Budget.Application.Shared.Models;
using Budget.Domain.Accounts;
using Budget.Domain.Budgets;
using Budget.Domain.Categories;
using Budget.Domain.Subcategories;
using Models.DataTypes;
using Models.Responses;

namespace Budget.Application.Subcategories.GetToAssign;

internal sealed class GetToAssignQueryHandler : IQueryHandler<GetToAssignQuery, MoneyModel>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly ISubcategoryRepository _subcategoryRepository;
    private readonly IAccountRepository _accountRepository;
    private readonly ICurrencyConverter _currencyConverter;
    private readonly IBudgetContext _budgetContext;
    private readonly IBudgetRepository _budgetRepository;

    public GetToAssignQueryHandler(IAccountRepository accountRepository, ICategoryRepository categoryRepository, ICurrencyConverter currencyConverter, IBudgetContext budgetContext, IBudgetRepository budgetRepository, ISubcategoryRepository subcategoryRepository)
    {
        _accountRepository = accountRepository;
        _categoryRepository = categoryRepository;
        _currencyConverter = currencyConverter;
        _budgetContext = budgetContext;
        _budgetRepository = budgetRepository;
        _subcategoryRepository = subcategoryRepository;
    }

    public async Task<Result<MoneyModel>> Handle(GetToAssignQuery request, CancellationToken cancellationToken)
    {
        var accountsGetResult = await _accountRepository.GetAllAsync(cancellationToken);

        if (accountsGetResult.IsFailure)
        {
            return accountsGetResult.Error;
        }

        var categoriesGetResult = await _categoryRepository.GetAllAsync(cancellationToken);

        if (categoriesGetResult.IsFailure)
        {
            return categoriesGetResult.Error;
        }

        var accounts = accountsGetResult.Value;
        var categories = categoriesGetResult.Value;

        var getBudgetIdResult = _budgetContext.GetBudgetId();

        if (getBudgetIdResult.IsFailure)
        {
            return getBudgetIdResult.Error;
        }

        var budgetId = new BudgetId(getBudgetIdResult.Value);

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

        var subcategoriesGetResult = await _subcategoryRepository.GetManyByIdAsync(
            categories.SelectMany(c => c.SubcategoriesId),
            cancellationToken);

        if (subcategoriesGetResult.IsFailure)
        {
            return subcategoriesGetResult.Error;
        }

        var subcategories = subcategoriesGetResult.Value;

        var overallAssignedAmount = new Money(
            subcategories.SelectMany(s => s.Assignments).Select(a => a.AssignedAmount).Sum(m => m.Amount),
            budgetCurrency);

        var toAssign = overallBalance - overallAssignedAmount;

        return MoneyModel.FromDomainModel(toAssign);
    }
}