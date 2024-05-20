using Abstractions.Messaging;
using Budget.Application.Abstractions.Currency;
using Budget.Application.Subcategories.Models;
using Budget.Domain.Subcategories;
using Budget.Domain.Transactions;
using MediatR;
using Models.DataTypes;
using Models.Responses;

namespace Budget.Application.Subcategories.UpdateAssignment;

internal sealed class UpdateAssignmentCommandHandler : ICommandHandler<UpdateAssignmentCommand, SubcategoryModel>
{
    private readonly ISubcategoryRepository _subcategoryRepository;
    private readonly ITransactionRepository _transactionRepository;
    private readonly ICurrencyConverter _currencyConverter;

    public UpdateAssignmentCommandHandler(ISubcategoryRepository subcategoryRepository, ITransactionRepository transactionRepository, ICurrencyConverter currencyConverter)
    {
        _subcategoryRepository = subcategoryRepository;
        _transactionRepository = transactionRepository;
        _currencyConverter = currencyConverter;
    }

    public async Task<Result<SubcategoryModel>> Handle(UpdateAssignmentCommand request, CancellationToken cancellationToken)
    {
        var subcategoryId = new SubcategoryId(request.SubcategoryId);

        var subcategoryGetResult = await _subcategoryRepository.GetByIdAsync(subcategoryId, cancellationToken);

        if (subcategoryGetResult.IsFailure)
        {
            return Result.Failure<SubcategoryModel>(subcategoryGetResult.Error);
        }

        var subcategory = subcategoryGetResult.Value;

        var assignmentBeforeUpdate = subcategory.GetAssignmentForMonth(request.AssignmentMonth);

        var budgetCurrency = Currency.Usd; //TODO Fix when authentication implemented

        var assignedAmountMoney = new Money(request.AssignedAmount, budgetCurrency);

        var assignmentResult = SubcategoryService.UpdateAssignment(
            subcategory,
            request.AssignmentMonth,
            assignedAmountMoney);

        if (assignmentResult.IsFailure)
        {
            return Result.Failure<SubcategoryModel>(assignmentResult.Error);
        }

        if (assignmentBeforeUpdate is not null)
        {
            return await _subcategoryRepository.UpdateAsync(subcategory, cancellationToken) is var result &&
                   result.IsFailure ?
                result.Error :
                SubcategoryModel.FromDomainModel(result.Value);
        }

        var assignmentTransactionsAddResult = await AddExistingTransactionsForAssignment(
            subcategory,
            assignmentResult.Value,
            cancellationToken);

        if (assignmentTransactionsAddResult.IsFailure)
        {
            return assignmentTransactionsAddResult.Error;
        }

        var subcategoryUpdateResult = await _subcategoryRepository.UpdateAsync(subcategory, cancellationToken);

        if (subcategoryUpdateResult.IsFailure)
        {
            return subcategoryUpdateResult.Error;
        }

        subcategory = subcategoryUpdateResult.Value;

        return Result.Success(SubcategoryModel.FromDomainModel(subcategory));
    }

    private async Task<Result> AddExistingTransactionsForAssignment(
        Subcategory subcategory,
        Assignment assignment,
        CancellationToken cancellationToken)
    {
        var relatedTransactionsGetResult = await _transactionRepository.GetWhereAsync(
            t => t.SubcategoryId != null &&
                 t.SubcategoryId == subcategory.Id &&
                 t.TransactedAt.Month == assignment.Month.Month &&
                 t.TransactedAt.Year == assignment.Month.Year, 
            cancellationToken);

        if (relatedTransactionsGetResult.IsFailure)
        {
            return relatedTransactionsGetResult.Error;
        }

        var relatedTransactions = relatedTransactionsGetResult.Value.ToArray();

        var assignmentTransactResults = relatedTransactions.Select(subcategory.TransactForAssignment);

        if (Result.Aggregate(assignmentTransactResults) is var result && result.IsFailure)
        {
            return result.Error;
        }

        return Result.Success();
    }
}