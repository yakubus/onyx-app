using Budget.Domain.Accounts;
using Budget.Domain.Counterparties;
using Budget.Domain.Subcategories;
using Models.DataTypes;
using Models.Responses;

namespace Budget.Domain.Transactions;

public sealed class TransactionFactory
{
    private readonly Account _account;
    private readonly Counterparty _counterparty;
    private readonly Subcategory? _subcategory;
    private readonly DateTime _transactedAt;
    private readonly Money _originalAmount;
    private readonly Money? _convertedAmount;
    private readonly Money? _assignmentAmount;
    private readonly Money? _targetAmount;
    private bool IsForeignTransaction => _convertedAmount is not null;
    private bool IsOutflow => _originalAmount < 0;

    public TransactionFactory(Account account, Counterparty counterparty, Subcategory? subcategory, DateTime transactedAt, Money originalAmount, Money? convertedAmount, Money? assignmentAmount, Money? targetAmount)
    {
        _account = account;
        _counterparty = counterparty;
        _subcategory = subcategory;
        _transactedAt = transactedAt;
        _originalAmount = originalAmount;
        _convertedAmount = convertedAmount;
        _assignmentAmount = assignmentAmount;
        _targetAmount = targetAmount;
    }

    public Result<Transaction> CreateTransaction() => 
        SwitchTransactionCreate(IsForeignTransaction, IsOutflow);

    private Result<Transaction>
        SwitchTransactionCreate(bool isForeignTransaction, bool isOutflow) =>
        (isForeignTransaction, isOutflow) switch
        {
            (true, true) => Transaction.CreateForeignOutflow(
                _account,
                _subcategory!,
                _convertedAmount!,
                _originalAmount,
                _transactedAt,
                _counterparty,
                _assignmentAmount ?? _originalAmount,
                _targetAmount ?? _originalAmount),
            (true, false) => Transaction.CreateForeignInflow(
                _account,
                _convertedAmount!,
                _originalAmount,
                _transactedAt,
                _counterparty),
            (false, true) => Transaction.CreatePrincipalOutflow(
                _account,
                _subcategory!,
                _originalAmount,
                _transactedAt,
                _counterparty,
                _assignmentAmount ?? _originalAmount,
                _targetAmount ?? _originalAmount),
            (false, false) => Transaction.CreatePrincipalInflow(
                _account,
                _originalAmount,
                _transactedAt,
                _counterparty)
        };
}