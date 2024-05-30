using Models.Responses;
using System.Linq.Expressions;

namespace Budget.Domain.Counterparties;

public interface ICounterpartyRepository
{
    Task<Result<Counterparty>> AddAsync(Counterparty counterparty, CancellationToken cancellationToken);

    Result<Counterparty> GetFirst(
        Expression<Func<Counterparty, bool>> filterPredicate,
        CancellationToken cancellationToken);

    Task<Result> RemoveAsync(CounterpartyId counterpartyId, CancellationToken cancellationToken = default);

    Task<Result<Counterparty>> GetByIdAsync(CounterpartyId counterpartyId, CancellationToken cancellationToken);

    Task<Result<Counterparty>> UpdateAsync(Counterparty counterparty, CancellationToken cancellationToken);

    Result<IEnumerable<Counterparty>> GetWhere(Expression<Func<Counterparty, bool>> filterPredicate, CancellationToken cancellationToken);

    Task<Result<IEnumerable<Counterparty>>> GetManyByIdAsync(
        IEnumerable<CounterpartyId> ids,
        CancellationToken cancellationToken = default);
}