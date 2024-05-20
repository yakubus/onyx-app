using Abstractions.Messaging;
using Budget.Application.Abstractions.Currency;
using Budget.Domain.Subcategories;
using Budget.Domain.Subcategories.DomainEvents;
using Budget.Domain.Transactions;
using Models.DataTypes;
using Models.Responses;

namespace Budget.Application.Subcategories.DomainEventHandlers.SubcategoryAssignedForMonth;

internal sealed class SubcategoryAssignedForMonthDomainEventHandler :
    IDomainEventHandler<SubcategoryAssignedForMonthDomainEvent>
{
    private readonly ISubcategoryRepository _subcategoryRepository;
    private readonly ITransactionRepository _transactionRepository;
    private readonly ICurrencyConverter _currencyConverter;

    public SubcategoryAssignedForMonthDomainEventHandler(ISubcategoryRepository subcategoryRepository, ITransactionRepository transactionRepository, ICurrencyConverter currencyConverter)
    {
        _subcategoryRepository = subcategoryRepository;
        _transactionRepository = transactionRepository;
        _currencyConverter = currencyConverter;
    }
    //TODO Money Exchange & Convert into serivce
    public async Task Handle(SubcategoryAssignedForMonthDomainEvent notification, CancellationToken cancellationToken)
    {
        var subcategoryGetResult = await _subcategoryRepository.GetByIdAsync(notification.SubcategoryId, cancellationToken);

        if (subcategoryGetResult.IsFailure)
        {
            return;
        }

        var subcategory = subcategoryGetResult.Value;

        var assignment = subcategory.GetAssignmentForMonth(notification.AssignedMonth);

        if (assignment is null)
        {
            return;
        }

        var transactionsInMonthGetResult = await _transactionRepository.GetWhereAsync(
            t => t.TransactedAt.Month == notification.AssignedMonth.Month &&
                 t.TransactedAt.Year == notification.AssignedMonth.Year,
            cancellationToken);

        if (transactionsInMonthGetResult.IsFailure)
        {
            return;
        }

        var transactionsInMonth = transactionsInMonthGetResult.Value.ToArray();

        var convertAmountResults = await Task.WhenAll(
            transactionsInMonth.Select(
                t => ConvertTransactionAssignmentAmount(
                    t,
                    assignment.ActualAmount.Currency,
                    cancellationToken)));

        if(Result.Aggregate(convertAmountResults).IsFailure)
        {
            return;
        }

        var transactResults = transactionsInMonth.Select(subcategory.TransactForAssignment);

        if(transactResults.Any(t => t.IsFailure))
        {
            return;
        }

        await _subcategoryRepository.UpdateAsync(subcategory, cancellationToken);
        await _transactionRepository.UpdateRangeAsync(transactionsInMonth, cancellationToken);
    }

    private async Task<Result<Money>> ConvertTransactionAssignmentAmount(
        Transaction transaction,
        Currency assignmentCurrency,
        CancellationToken cancellationToken)
    {
        var convertResult = await _currencyConverter.ConvertMoney(
            transaction.Amount,
            assignmentCurrency,
            cancellationToken);

        if (convertResult.IsFailure)
        {
            return convertResult.Error;
        }

        transaction.SetAssignmentAmount(convertResult.Value);

        return convertResult;
    }
}