using Models.Responses;

namespace Budget.Domain.Counterparties;

public interface ICounterpartyRepository
{
    Task<Result<Counterparty>> AddAsync(Counterparty counterparty, CancellationToken cancellationToken);

    Task<Result> RemoveAsync(CounterpartyId counterpartyId, CancellationToken cancellationToken = default);

    Task<Result<Counterparty>> GetByIdAsync(CounterpartyId counterpartyId, CancellationToken cancellationToken);

    Task<Result<Counterparty>> UpdateAsync(Counterparty counterparty, CancellationToken cancellationToken);

    Task<Result<IEnumerable<Counterparty>>> GetManyByIdAsync(
        IEnumerable<CounterpartyId> ids,
        CancellationToken cancellationToken = default);

    Task<Result<IEnumerable<Counterparty>>> GetAllAsync(CancellationToken cancellationToken);

    Task<Result<Counterparty>> GetByNameAndType(CounterpartyName counterpartyName, CounterpartyType counterpartyType, CancellationToken cancellationToken);

    Task<Result<IEnumerable<Counterparty>>> GetByType(CounterpartyType counterpartyType, CancellationToken cancellationToken);
}