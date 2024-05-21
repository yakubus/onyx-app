using System.Net;
using Abstractions.DomainBaseTypes;
using Budget.Domain.Accounts;
using Budget.Domain.Categories;
using Budget.Domain.Counterparties;
using Budget.Domain.Subcategories;
using Budget.Domain.Transactions;
using Budget.Infrastructure.Data.DataSettings;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;

namespace Budget.Infrastructure.Data;

internal sealed class CosmosDbContext
{
    private readonly Database _database;

    public CosmosDbContext(IOptions<CosmosDbOptions> options)
    {
        var cosmosOptions = options.Value;

        var client = new CosmosClient(cosmosOptions.AccountUri, cosmosOptions.PrimaryKey);
        _database = client.GetDatabase(cosmosOptions.Database);
    }

    private readonly CosmosException _connectionException = new(
        "Problem while connecting to CosmosDB",
        HttpStatusCode.InternalServerError,
        500,
        "connection",
        1);

    private Container Accounts => _database.GetContainer(nameof(Accounts)) 
                                  ?? throw _connectionException;
    private Container Counterparties => _database.GetContainer(nameof(Counterparties)) 
                                        ?? throw _connectionException;
    private Container Subcategories => _database.GetContainer(nameof(Subcategories)) 
                                       ?? throw _connectionException;
    private Container Categories => _database.GetContainer(nameof(Categories)) 
                                    ?? throw _connectionException;
    private Container Transactions => _database.GetContainer(nameof(Transactions)) 
                                      ?? throw _connectionException;

    internal Container Set<T>() where T : IEntity
    {
        return typeof(T) switch
        {
            var type when type == typeof(Category) => Categories,
            var type when type == typeof(Subcategory) => Subcategories,
            var type when type == typeof(Transaction) => Transactions,
            var type when type == typeof(Account) => Accounts,
            var type when type == typeof(Counterparty) => Counterparties,
            _ => throw new ArgumentException("Unknown entity type")
        };
    }
}