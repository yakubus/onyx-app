using Budget.Application.Abstractions.Identity;
using Budget.Domain.Counterparties;
using SharedDAL;
using SharedDAL.DataModels.Abstractions;

namespace Budget.Infrastructure.Repositories;

internal sealed class CounterpartyRepository : BaseBudgetRepository<Counterparty, CounterpartyId>, ICounterpartyRepository
{
    public CounterpartyRepository(
        DbContext context,
        IBudgetContext budgetContext,
        IDataModelService<Counterparty> dataModelService) : base(
        context,
        budgetContext,
        dataModelService)
    {
    }
}