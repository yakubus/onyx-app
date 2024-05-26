using Budget.Domain.Transactions;
using SharedDAL;


namespace Budget.Infrastructure.Repositories;

internal sealed class TransactionRepository : Repository<Transaction, TransactionId>, ITransactionRepository
{
    public TransactionRepository(CosmosDbContext context) : base(context)
    {
    }
}