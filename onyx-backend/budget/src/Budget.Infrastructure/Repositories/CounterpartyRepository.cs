using Budget.Domain.Counterparties;
using Budget.Infrastructure.Data;

namespace Budget.Infrastructure.Repositories;

internal sealed class CounterpartyRepository : Repository<Counterparty, CounterpartyId>, ICounterpartyRepository
{
    public CounterpartyRepository(CosmosDbContext context) : base(context)
    {
    }
}