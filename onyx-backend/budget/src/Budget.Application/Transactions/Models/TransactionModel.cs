using System.Text.Json.Serialization;
using Abstractions.Messaging;
using Budget.Application.Accounts.Models;
using Budget.Application.Counterparties.Models;
using Budget.Application.Shared.Models;
using Budget.Application.Subcategories.Models;
using Budget.Domain.Accounts;
using Budget.Domain.Counterparties;
using Budget.Domain.Subcategories;
using Budget.Domain.Transactions;

namespace Budget.Application.Transactions.Models;

public sealed record TransactionModel : EntityBusinessModel
{
    public Guid Id { get; init; }
    public AccountModel Account { get; init; }
    public MoneyModel Amount { get; init; }
    public MoneyModel? OriginalAmount { get; init; }
    public SubcategoryModel? Subcategory { get; init; }
    public CounterpartyModel Counterparty { get; init; }
    public DateTime TransactedAt { get; init; }

    [JsonConstructor]
    private TransactionModel(
        Guid id,
        AccountModel account,
        MoneyModel amount,
        MoneyModel? originalAmount,
        SubcategoryModel? subcategory,
        CounterpartyModel counterparty,
        DateTime transactedAt,
        IEnumerable<IDomainEvent> domainEvents) : base(domainEvents)
    {
            Id = id;
            Account = account;
            Amount = amount;
            OriginalAmount = originalAmount;
            Subcategory = subcategory;
            Counterparty = counterparty;
            TransactedAt = transactedAt;
        }

    public static TransactionModel FromDomainModel(
        Transaction domainModel,
        Counterparty counterparty,
        Account account,
        Subcategory? subcategory) =>
        new (
            domainModel.Id.Value,
            AccountModel.FromDomainModel(account),
            MoneyModel.FromDomainModel(domainModel.Amount),
            domainModel.OriginalAmount == null ? 
                null : 
                MoneyModel.FromDomainModel(domainModel.OriginalAmount),
            subcategory == null ? 
                null : 
                SubcategoryModel.FromDomainModel(subcategory),
            CounterpartyModel.FromDomainModel(counterparty),
            domainModel.TransactedAt,
            domainModel.GetDomainEvents());
}