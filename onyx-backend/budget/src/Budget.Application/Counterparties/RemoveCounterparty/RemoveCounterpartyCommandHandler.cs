using Abstractions.Messaging;
using Budget.Domain.Counterparties;
using Models.Responses;

namespace Budget.Application.Counterparties.RemoveCounterparty;

internal sealed class RemoveCounterpartyCommandHandler : ICommandHandler<RemoveCounterpartyCommand>
{
    private readonly ICounterpartyRepository _counterpartyRepository;

    public RemoveCounterpartyCommandHandler(ICounterpartyRepository counterpartyRepository)
    {
        _counterpartyRepository = counterpartyRepository;
    }

    //TODO: Remove related transactions!!!
    public async Task<Result> Handle(
        RemoveCounterpartyCommand request,
        CancellationToken cancellationToken) =>
        await _counterpartyRepository.RemoveAsync(
            new(request.Id),
            cancellationToken);
}