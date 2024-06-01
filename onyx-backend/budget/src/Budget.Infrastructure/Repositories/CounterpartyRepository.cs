using Budget.Application.Abstractions.Identity;
using Budget.Domain.Counterparties;
using SharedDAL;

namespace Budget.Infrastructure.Repositories;

internal sealed class CounterpartyRepository : BaseBudgetRepository<Counterparty, CounterpartyId>, ICounterpartyRepository
{
    public CounterpartyRepository(DbContext context, IBudgetContext budgetContext) : base(context, budgetContext)
    {
    }
}