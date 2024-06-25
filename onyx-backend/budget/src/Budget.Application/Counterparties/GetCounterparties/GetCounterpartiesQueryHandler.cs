using Abstractions.Messaging;
using Budget.Application.Counterparties.Models;
using Budget.Domain.Counterparties;
using Models.Responses;

namespace Budget.Application.Counterparties.GetCounterparties;

internal sealed class GetCounterpartiesQueryHandler : IQueryHandler<GetCounterpartiesQuery, IEnumerable<CounterpartyModel>>
{
    private readonly ICounterpartyRepository _counterpartyRepository;

    public GetCounterpartiesQueryHandler(ICounterpartyRepository counterpartyRepository)
    {
        _counterpartyRepository = counterpartyRepository;
    }

    public async Task<Result<IEnumerable<CounterpartyModel>>> Handle(GetCounterpartiesQuery request, CancellationToken cancellationToken)
    {
        var counterPartyTypeCreateResult = CounterpartyType.Create(request.CounterpartyType);

        if (counterPartyTypeCreateResult.IsFailure)
        {
            return counterPartyTypeCreateResult.Error;
        }

        var counterpartyType = counterPartyTypeCreateResult.Value;

        var counterpartiesGetResult = await _counterpartyRepository.GetByType(
            counterpartyType,
            cancellationToken);

        if (counterpartiesGetResult.IsFailure)
        {
            return counterpartiesGetResult.Error;
        }

        var counterparties = counterpartiesGetResult.Value;

        return Result.Create(counterparties.Select(CounterpartyModel.FromDomainModel));
    }
}