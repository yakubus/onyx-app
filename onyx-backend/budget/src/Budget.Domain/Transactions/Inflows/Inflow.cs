using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Budget.Domain.Accounts;
using Budget.Domain.Counterparties.Payees;
using Budget.Domain.Counterparties.Payers;
using Budget.Domain.Subcategories;
using Budget.Domain.Transactions.Outflows;
using Models.DataTypes;
using Models.Responses;

namespace Budget.Domain.Transactions.Inflows;

public sealed class Inflow : Transaction
{
    public Payer Payer { get; init; }

    private Inflow(Account account, Subcategory subcategory, Money amount, DateTime transactedAt, Payer payer, Money? originalAmount) 
        : base(account, subcategory, amount, transactedAt, originalAmount)
    {
        Payer = payer;
    }

    public static Result<Inflow> CreatePrincipal(
        Account account,
        Subcategory subcategory,
        Money amount,
        DateTime transactedAt,
        Payer payer)
    {
        var transaction = new Inflow(account, subcategory, amount, transactedAt, payer, null);

        var accountTransactResult = account.Transact(transaction);

        if (accountTransactResult.IsFailure)
        {
            return Result.Failure<Inflow>(accountTransactResult.Error);
        }

        var subcategoryTransactResult = subcategory.Transact(transaction);

        if (subcategoryTransactResult.IsFailure)
        {
            return Result.Failure<Inflow>(subcategoryTransactResult.Error);
        }

        return transaction;
    }

    public static Result<Inflow> CreateForeign(
        Account account,
        Subcategory subcategory,
        Money convertedAmount,
        Money originalAmount,
        DateTime transactedAt,
        Payee payer)
    {
        if (account.Balance.Currency == originalAmount.Currency)
        {
            return Result.Failure<Inflow>(TransactionErrors.TransactionIsNotForeign);
        }

        var transaction = new Inflow(account, subcategory, convertedAmount, transactedAt, payer, originalAmount);

        var accountTransactResult = account.Transact(transaction);

        if (accountTransactResult.IsFailure)
        {
            return Result.Failure<Inflow>(accountTransactResult.Error);
        }

        var subcategoryTransactResult = subcategory.Transact(transaction);

        if (subcategoryTransactResult.IsFailure)
        {
            return Result.Failure<Inflow>(subcategoryTransactResult.Error);
        }

        return transaction;
    }
}