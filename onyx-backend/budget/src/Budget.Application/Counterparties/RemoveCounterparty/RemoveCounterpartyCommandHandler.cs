using Abstractions.Messaging;
using Budget.Domain.Counterparties;
using Budget.Domain.Subcategories;
using Models.Responses;

namespace Budget.Application.Counterparties.RemoveCounterparty;

internal sealed class RemoveCounterpartyCommandHandler : ICommandHandler<RemoveCounterpartyCommand>
{
    private readonly ICounterpartyRepository _counterpartyRepository;
    private readonly ISubcategoryRepository _subcategoryRepository;

    public RemoveCounterpartyCommandHandler(ICounterpartyRepository counterpartyRepository, ISubcategoryRepository subcategoryRepository)
    {
        _counterpartyRepository = counterpartyRepository;
        _subcategoryRepository = subcategoryRepository;
    }

    public async Task<Result> Handle(RemoveCounterpartyCommand request, CancellationToken cancellationToken)
    {
        var counterpartyId = new CounterpartyId(request.Id);

        var removeCounterpartyResult = await _counterpartyRepository.RemoveAsync(
            counterpartyId,
            cancellationToken);

        if (removeCounterpartyResult.IsFailure)
        {
            return Result.Failure(removeCounterpartyResult.Error);
        }

        return Result.Success();
    }
}