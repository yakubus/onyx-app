using Budget.Application.Abstractions.Identity;
using Budget.Domain.Transactions;
using SharedDAL;


namespace Budget.Infrastructure.Repositories;

internal sealed class TransactionRepository : BaseBudgetRepository<Transaction, TransactionId>, ITransactionRepository
{
    public TransactionRepository(DbContext context, IBudgetContext budgetContext) : base(context, budgetContext)
    {
    }
}