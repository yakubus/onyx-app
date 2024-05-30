using Abstractions.Messaging;
using Budget.Application.Counterparties.Models;
using Budget.Domain.Counterparties;
using Budget.Domain.Transactions;
using Models.Responses;

namespace Budget.Application.Counterparties.UpdateCounterparty;

internal sealed class UpdateCounterpartyCommandHandler : ICommandHandler<UpdateCounterpartyCommand, CounterpartyModel>
{
    private readonly ICounterpartyRepository _counterpartyRepository;
    private readonly ITransactionRepository _transactionRepository;

    public UpdateCounterpartyCommandHandler(ITransactionRepository transactionRepository, ICounterpartyRepository counterpartyRepository)
    {
        _transactionRepository = transactionRepository;
        _counterpartyRepository = counterpartyRepository;
    }

    public async Task<Result<CounterpartyModel>> Handle(UpdateCounterpartyCommand request, CancellationToken cancellationToken)
    {
        var counterpartyId = new CounterpartyId(request.Id);

        var counterpartyGetResult = await _counterpartyRepository.GetByIdAsync(counterpartyId, cancellationToken);
        
        if (counterpartyGetResult.IsFailure)
        {
            return Result.Failure<CounterpartyModel>(counterpartyGetResult.Error);
        }

        var counterparty = counterpartyGetResult.Value;

        var changeNameResult = counterparty.ChangeName(request.NewName);

        if (changeNameResult.IsFailure)
        {
            return Result.Failure<CounterpartyModel>(changeNameResult.Error);
        }

        var isCounterpartyExistsResult = _counterpartyRepository.GetFirst(
            c => c.Name == counterparty.Name && c.Type == counterparty.Type,
            cancellationToken);

        if (isCounterpartyExistsResult.IsSuccess)
        {
            return Result.Failure<CounterpartyModel>(UpdateCounterpartyErrors.CounterpartyAlreadyExists);
        }

        var counterpartyUpdateResult = await _counterpartyRepository.UpdateAsync(counterparty, cancellationToken);

        if (counterpartyUpdateResult.IsFailure)
        {
            return Result.Failure<CounterpartyModel>(counterpartyUpdateResult.Error);
        }

        counterparty = counterpartyUpdateResult.Value;

        return CounterpartyModel.FromDomainModel(counterparty);
    }
}