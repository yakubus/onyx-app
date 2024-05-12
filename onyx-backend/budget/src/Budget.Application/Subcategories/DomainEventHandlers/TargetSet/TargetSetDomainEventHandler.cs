using Abstractions.Messaging;
using Budget.Domain.Subcategories;
using Budget.Domain.Subcategories.DomainEvents;
using Budget.Domain.Transactions;

namespace Budget.Application.Subcategories.DomainEventHandlers.TargetSet;

internal sealed class TargetSetDomainEventHandler : IDomainEventHandler<TargetSetDomainEvent>
{
    private readonly ISubcategoryRepository _subcategoryRepository;
    private readonly ITransactionRepository _transactionRepository;

    public TargetSetDomainEventHandler(ISubcategoryRepository subcategoryRepository, ITransactionRepository transactionRepository)
    {
        _subcategoryRepository = subcategoryRepository;
        _transactionRepository = transactionRepository;
    }

    public async Task Handle(TargetSetDomainEvent notification, CancellationToken cancellationToken)
    {
        var subcategoryGetResult = await _subcategoryRepository.GetByIdAsync(notification.SubcategoryId, cancellationToken);

        if (subcategoryGetResult.IsFailure)
        {
            return;
        }

        var subcategory = subcategoryGetResult.Value;
        var targetFromMonth = subcategory.Target?.StartedAt;
        var targetToMonth = subcategory.Target?.UpToMonth;

        if (targetFromMonth is null || targetToMonth is null)
        {
            return;
        }

        var transactionsForTargetGetResult = await _transactionRepository.GetWhereAsync(
            t => t.SubcategoryId == subcategory.Id &&
                t.TransactedAt.Month >= targetFromMonth.Month &&
                 t.TransactedAt.Year >= targetFromMonth.Year &&
                t.TransactedAt.Month <= targetToMonth.Month &&
                 t.TransactedAt.Year <= targetToMonth.Year,
            cancellationToken);

        if(transactionsForTargetGetResult.IsFailure)
        {
            return;
        }

        var transactionsForTarget = transactionsForTargetGetResult.Value;

        var transactResults = transactionsForTarget.Select(subcategory.TransactForTarget);

        if (transactResults.Any(r => r.IsFailure))
        {
            return;
        }

        await _subcategoryRepository.UpdateAsync(subcategory, cancellationToken);
    }
}