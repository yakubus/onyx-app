using Abstractions.Messaging;
using Budget.Domain.Subcategories;
using Budget.Domain.Subcategories.DomainEvents;
using Budget.Domain.Transactions;

namespace Budget.Application.Subcategories.DomainEventHandlers.SubcategoryAssignedForMonth;

internal sealed class SubcategoryAssignedForMonthDomainEventHandler :
    IDomainEventHandler<SubcategoryAssignedForMonthDomainEvent>
{
    private readonly ISubcategoryRepository _subcategoryRepository;
    private readonly ITransactionRepository _transactionRepository;

    public SubcategoryAssignedForMonthDomainEventHandler(ISubcategoryRepository subcategoryRepository, ITransactionRepository transactionRepository)
    {
        _subcategoryRepository = subcategoryRepository;
        _transactionRepository = transactionRepository;
    }

    public async Task Handle(SubcategoryAssignedForMonthDomainEvent notification, CancellationToken cancellationToken)
    {
        var subcategoryGetResult = await _subcategoryRepository.GetByIdAsync(notification.SubcategoryId, cancellationToken);

        if (subcategoryGetResult.IsFailure)
        {
            return;
        }

        var subcategory = subcategoryGetResult.Value;

        var transactionsInMonthGetResult = await _transactionRepository.GetWhereAsync(
            t => t.TransactedAt.Month == notification.AssignedMonth.Month &&
                 t.TransactedAt.Year == notification.AssignedMonth.Year,
            cancellationToken);

        if (transactionsInMonthGetResult.IsFailure)
        {
            return;
        }

        var transactionsInMonth = transactionsInMonthGetResult.Value;

        var transactResults = transactionsInMonth.Select(subcategory.TransactForAssignment);

        if(transactResults.Any(t => t.IsFailure))
        {
            return;
        }

        await _subcategoryRepository.UpdateAsync(subcategory, cancellationToken);
    }
}