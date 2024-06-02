using Abstractions.Messaging;
using Budget.Application.Counterparties.Models;
using Budget.Domain.Counterparties;
using Models.Responses;

namespace Budget.Application.Counterparties.AddCounterparty;

internal sealed class AddCounterpartyCommandHandler : ICommandHandler<AddCounterpartyCommand, CounterpartyModel>
{
    private readonly ICounterpartyRepository _counterpartyRepository;

    public AddCounterpartyCommandHandler(ICounterpartyRepository counterpartyRepository)
    {
        _counterpartyRepository = counterpartyRepository;
    }

    public async Task<Result<CounterpartyModel>> Handle(AddCounterpartyCommand request, CancellationToken cancellationToken)
    {
        var counterpartyCreateResult = Counterparty.Create(request.CounterpartyName, request.CounterpartyType, new (request.BudgetId));

        if (counterpartyCreateResult.IsFailure)
        {
            return Result.Failure<CounterpartyModel>(counterpartyCreateResult.Error);
        }

        var counterparty = counterpartyCreateResult.Value;

        var isCounterpartyExistsResult = _counterpartyRepository.GetFirst(
            c => c.Name == counterparty.Name && c.Type == counterparty.Type, 
            cancellationToken);

        if (isCounterpartyExistsResult.IsSuccess)
        {
            return Result.Failure<CounterpartyModel>(AddCounterpartyErrors.CounterpartyAlreadyExists);
        }

        var addResult = await _counterpartyRepository.AddAsync(counterparty, cancellationToken);

        if (addResult.IsFailure)
        {
            return Result.Failure<CounterpartyModel>(addResult.Error);
        }

        counterparty = addResult.Value;

        return CounterpartyModel.FromDomainModel(counterparty);
    }
}