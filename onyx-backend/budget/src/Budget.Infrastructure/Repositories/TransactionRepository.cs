using Amazon.DynamoDBv2.DocumentModel;
using Budget.Application.Abstractions.Identity;
using Budget.Application.Transactions.GetTransactions;
using Budget.Domain.Accounts;
using Budget.Domain.Counterparties;
using Budget.Domain.Subcategories;
using Budget.Domain.Transactions;
using Budget.Infrastructure.Data.DataModels.Transactions;
using Models.Responses;
using SharedDAL;
using SharedDAL.DataModels.Abstractions;


namespace Budget.Infrastructure.Repositories;

internal sealed class TransactionRepository : BaseBudgetRepository<Transaction, TransactionId>, ITransactionRepository
{
    public TransactionRepository(
        DbContext context,
        IBudgetContext budgetContext,
        IDataModelService<Transaction> dataModelService) : base(
        context,
        budgetContext,
        dataModelService)
    {
    }

    public async Task<Result<IEnumerable<Transaction>>> GetByAccountAsync(AccountId accountId, CancellationToken cancellationToken)
    {
        var scanFilter = new ScanFilter();
        scanFilter.AddCondition(nameof(TransactionDataModel.AccountId), ScanOperator.Equal, accountId.Value);

        return await GetWhereAsync(scanFilter, cancellationToken);
    }

    public async Task<Result<IEnumerable<Transaction>>> GetByCounterpartyAsync(CounterpartyId counterpartyId, CancellationToken cancellationToken)
    {
        var scanFilter = new ScanFilter();
        scanFilter.AddCondition(nameof(TransactionDataModel.CounterpartyId), ScanOperator.Equal, counterpartyId.Value);

        return await GetWhereAsync(scanFilter, cancellationToken);
    }

    public async Task<Result<IEnumerable<Transaction>>> GetForAssignmentAsync(
        SubcategoryId subcategoryId,
        Assignment assignment,
        CancellationToken cancellationToken)
    {
        var scanFilter = new ScanFilter();
        scanFilter.AddCondition(nameof(TransactionDataModel.SubcategoryId), ScanOperator.Equal, subcategoryId.Value);
        scanFilter.AddCondition(nameof(TransactionDataModel.TransactedAtMonth), ScanOperator.Equal, assignment.Month.Month);
        scanFilter.AddCondition(nameof(TransactionDataModel.TransactedAtYear), ScanOperator.Equal, assignment.Month.Year);

        return await GetWhereAsync(scanFilter, cancellationToken);
    }

    public async Task<Result<IEnumerable<Transaction>>> GetBySubcategoryAsync(SubcategoryId subcategoryId, CancellationToken cancellationToken)
    {
        var scanFilter = new ScanFilter();
        scanFilter.AddCondition(nameof(TransactionDataModel.SubcategoryId), ScanOperator.Equal, subcategoryId.Value);

        return await GetWhereAsync(scanFilter, cancellationToken);
    }

    public async Task<Result<IEnumerable<Transaction>>> GetForTargetAsync(SubcategoryId subcategoryId, Target target, CancellationToken cancellationToken)
    {
        var scanFilter = new ScanFilter();
        scanFilter.AddCondition(nameof(TransactionDataModel.SubcategoryId), ScanOperator.Equal, subcategoryId.Value);
        scanFilter.AddCondition(nameof(TransactionDataModel.TransactedAtMonth), ScanOperator.LessThanOrEqual, target.UpToMonth.Month);
        scanFilter.AddCondition(nameof(TransactionDataModel.TransactedAtYear), ScanOperator.LessThanOrEqual, target.UpToMonth.Year);
        scanFilter.AddCondition(nameof(TransactionDataModel.TransactedAtMonth), ScanOperator.GreaterThanOrEqual, target.StartedAt.Month);
        scanFilter.AddCondition(nameof(TransactionDataModel.TransactedAtYear), ScanOperator.GreaterThanOrEqual, target.StartedAt.Year);
        
        return await GetWhereAsync(scanFilter, cancellationToken);
    }
}