using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Budget.Domain.Accounts;
using Budget.Domain.Counterparties.Payees;
using Budget.Domain.Subcategories;
using Models.DataTypes;
using Models.Responses;

namespace Budget.Domain.Transactions.Outflows;

public sealed class Outflow : Transaction
{
    public Payee Payee { get; init; }

    private Outflow(Account account, Subcategory subcategory, Money amount, DateTime transactedAt, Payee payee, Money? originalAmount) 
        : base(account, subcategory, amount, transactedAt, originalAmount)
    {
        Payee = payee;
    }

    public static Result<Outflow> CreatePrincipal(
        Account account,
        Subcategory subcategory,
        Money amount,
        DateTime transactedAt,
        Payee payee)
    {
        var transaction = new Outflow(account, subcategory, amount, transactedAt, payee, null);

        var accountTransactResult = account.Transact(transaction);

        if (accountTransactResult.IsFailure)
        {
            return Result.Failure<Outflow>(accountTransactResult.Error);
        }

        var subcategoryTransactResult = subcategory.Transact(transaction);

        if (subcategoryTransactResult.IsFailure)
        {
            return Result.Failure<Outflow>(subcategoryTransactResult.Error);
        }

        return transaction;
    }

    public static Result<Outflow> CreateForeign(
        Account account,
        Subcategory subcategory,
        Money convertedAmount,
        Money originalAmount,
        DateTime transactedAt,
        Payee payee)
    {
        if(account.Balance.Currency == originalAmount.Currency)
        {
            return Result.Failure<Outflow>(TransactionErrors.TransactionIsNotForeign);
        }

        var transaction = new Outflow(account, subcategory, convertedAmount, transactedAt, payee, originalAmount);

        var accountTransactResult = account.Transact(transaction);

        if (accountTransactResult.IsFailure)
        {
            return Result.Failure<Outflow>(accountTransactResult.Error);
        }

        var subcategoryTransactResult = subcategory.Transact(transaction);

        if (subcategoryTransactResult.IsFailure)
        {
            return Result.Failure<Outflow>(subcategoryTransactResult.Error);
        }

        return transaction;
    }
}