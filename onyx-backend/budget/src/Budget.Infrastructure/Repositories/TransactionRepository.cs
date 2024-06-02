using Budget.Application.Abstractions.Identity;
using Budget.Domain.Transactions;
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
}