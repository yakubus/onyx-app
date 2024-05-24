using Abstractions.Messaging;
using Budget.Domain.Counterparties;
using Budget.Domain.Transactions;
using Models.Responses;

namespace Budget.Application.Counterparties.RemoveCounterparty;

internal sealed class RemoveCounterpartyCommandHandler : ICommandHandler<RemoveCounterpartyCommand>
{
    private readonly ICounterpartyRepository _counterpartyRepository;
    private readonly ITransactionRepository _transactionRepository;

    public RemoveCounterpartyCommandHandler(ICounterpartyRepository counterpartyRepository, ITransactionRepository transactionRepository)
    {
        _counterpartyRepository = counterpartyRepository;
        _transactionRepository = transactionRepository;
    }

    public async Task<Result> Handle(RemoveCounterpartyCommand request, CancellationToken cancellationToken)
    {
        var counterpartyId = new CounterpartyId(request.Id);

        var relatedTransactionsGetResult = _transactionRepository.GetWhere(
            t => t.CounterpartyId == counterpartyId,
            cancellationToken);

        if (relatedTransactionsGetResult.IsFailure)
        {
            return Result.Failure(relatedTransactionsGetResult.Error);
        }

        var relatedTransactions = relatedTransactionsGetResult.Value.ToList();

        relatedTransactions.ForEach(t => t.RemoveCounterparty());

        var removeCounterpartyTask = _counterpartyRepository.RemoveAsync(
            counterpartyId,
            cancellationToken);
        var updateTransactionsTask = _transactionRepository.UpdateRangeAsync(
            relatedTransactions,
            cancellationToken);

        var results = await Task.WhenAll(removeCounterpartyTask, updateTransactionsTask);

        if (results.FirstOrDefault(r => r.IsFailure) is not null and var failure)
        {
            return Result.Failure(failure.Error);
        }

        return Result.Success();
    }
}