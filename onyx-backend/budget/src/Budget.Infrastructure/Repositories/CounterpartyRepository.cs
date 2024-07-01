using Amazon.DynamoDBv2.DocumentModel;
using Budget.Application.Abstractions.Identity;
using Budget.Domain.Counterparties;
using Budget.Infrastructure.Data.DataModels.Counterparties;
using Models.Responses;
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

    public async Task<Result<Counterparty>> GetByNameAndType(
        CounterpartyName counterpartyName,
        CounterpartyType counterpartyType,
        CancellationToken cancellationToken)
    {
        var scanFilter = new ScanFilter();
        scanFilter.AddCondition(
            nameof(CounterpartyDataModel.Name),
            ScanOperator.Equal,
            counterpartyName.Value);
        scanFilter.AddCondition(
            nameof(CounterpartyDataModel.Type),
            ScanOperator.Equal,
            counterpartyType.Value);

        return await GetFirstAsync(scanFilter, cancellationToken);
    }

    public async Task<Result<IEnumerable<Counterparty>>> GetByType(CounterpartyType counterpartyType, CancellationToken cancellationToken)
    {
        var scanFilter = new ScanFilter();
        scanFilter.AddCondition(
            nameof(CounterpartyDataModel.Type),
            ScanOperator.Equal,
            counterpartyType.Value);

        return await GetWhereAsync(scanFilter, cancellationToken);
    }
}