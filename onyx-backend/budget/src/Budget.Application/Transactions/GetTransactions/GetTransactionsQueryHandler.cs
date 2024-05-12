using Abstractions.Messaging;
using Budget.Application.Transactions.Models;
using Budget.Domain.Accounts;
using Budget.Domain.Counterparties;
using Budget.Domain.Subcategories;
using Budget.Domain.Transactions;
using Models.DataTypes;
using Models.Responses;
using Transaction = Budget.Domain.Transactions.Transaction;

namespace Budget.Application.Transactions.GetTransactions;

internal sealed class GetTransactionsQueryHandler : IQueryHandler<GetTransactionsQuery, IEnumerable<TransactionModel>>
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly ISubcategoryRepository _subcategoryRepository;
    private readonly IAccountRepository _accountRepository;
    private readonly ICounterpartyRepository _counterpartyRepository;

    public GetTransactionsQueryHandler(ITransactionRepository transactionRepository, ICounterpartyRepository counterpartyRepository, ISubcategoryRepository subcategoryRepository, IAccountRepository accountRepository)
    {
        _transactionRepository = transactionRepository;
        _counterpartyRepository = counterpartyRepository;
        _subcategoryRepository = subcategoryRepository;
        _accountRepository = accountRepository;
    }

    public async Task<Result<IEnumerable<TransactionModel>>> Handle(GetTransactionsQuery request, CancellationToken cancellationToken)
    {
        if (request.Query is null)
        {
            return Result.Failure<IEnumerable<TransactionModel>>(GetTransactionErrors.QueryIsNull);
        }

        var queryCreateResult = GetTransactionQueryRequest.FromString(request.Query);

        if (queryCreateResult.IsFailure)
        {
            return Result.Failure<IEnumerable<TransactionModel>>(queryCreateResult.Error);
        }

        var query = queryCreateResult.Value;

        var isRequestValid = IsQueryValid(query, request);

        if (!isRequestValid)
        {
            return Result.Failure<IEnumerable<TransactionModel>>(GetTransactionErrors.InvalidQueryValues);
        }

        var filter = GetTransactionFilters.GetFilter(query, request);

        var transactionsGetResult = await _transactionRepository.GetWhereAsync(filter, cancellationToken);

        if (transactionsGetResult.IsFailure)
        {
            return Result.Failure<IEnumerable<TransactionModel>>(transactionsGetResult.Error);
        }

        var transactions = transactionsGetResult.Value;

        var transactionModelsGetResults = await Task.WhenAll(
            GetTransactionModelsTasks(
                transactions,
                cancellationToken));

        if (transactionModelsGetResults.FirstOrDefault(r => r.IsFailure) is not null and var failureResult)
        {
            return Result.Failure<IEnumerable<TransactionModel>>(failureResult.Error);
        }

        var transactionModels = transactionModelsGetResults.Select(r => r.Value);

        return Result.Create(transactionModels);
    }

    private IEnumerable<Task<Result<TransactionModel>>> GetTransactionModelsTasks(
        IEnumerable<Transaction> transactions,
        CancellationToken cancellationToken)
    {
        var tasks = new List<Task<Result<TransactionModel>>>();

        foreach (var t in transactions)
        {
            var task = async () =>
            {
                var accountGetResult = await _accountRepository.GetByIdAsync(t.AccountId, cancellationToken);

                if (accountGetResult.IsFailure)
                {
                    return Result.Failure<TransactionModel>(accountGetResult.Error);
                }

                var subcategoryGetResult = t.SubcategoryId is null ?
                    null :
                    await _subcategoryRepository.GetByIdAsync(t.SubcategoryId, cancellationToken);

                if (subcategoryGetResult is not null && subcategoryGetResult.IsFailure)
                {
                    return Result.Failure<TransactionModel>(subcategoryGetResult.Error);
                }

                var counterpartyGetResult =
                    await _counterpartyRepository.GetByIdAsync(t.CounterpartyId, cancellationToken);

                if (counterpartyGetResult.IsFailure)
                {
                    return Result.Failure<TransactionModel>(counterpartyGetResult.Error);
                }

                var account = accountGetResult.Value;
                var subcategory = subcategoryGetResult?.Value;
                var counterparty = counterpartyGetResult.Value;

                return Result.Create(TransactionModel.FromDomainModel(t, counterparty, account, subcategory));
            };

            tasks.Add(task.Invoke());
        }

        return tasks;
    }

    private static bool IsQueryValid(GetTransactionQueryRequest query, GetTransactionsQuery request) =>
        query switch
        {
            _ when query == GetTransactionQueryRequest.All ||
                   query == GetTransactionQueryRequest.Empty =>
                true,
            _ when query == GetTransactionQueryRequest.Account =>
                request.AccountId is not null,
            _ when query == GetTransactionQueryRequest.Subcategory =>
                request.SubcategoryId is not null,
            _ when query == GetTransactionQueryRequest.Counterparty =>
                request.CounterpartyId is not null,
            _ => false
        };
}