using System.Net;
using Abstractions.DomainBaseTypes;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;
using SharedDAL.DataSettings;

namespace SharedDAL;

//TODO implement idisposable
public sealed class CosmosDbContext
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
    private Container Users => _database.GetContainer(nameof(Users)) 
                                      ?? throw _connectionException;
    private Container Budgets => _database.GetContainer(nameof(Budgets)) 
                                      ?? throw _connectionException;

    internal Container Set<T>() where T : IEntity
    {
        return typeof(T) switch
        {
            { Name: "Category" } => Categories,
            { Name: "Subcategory" } => Subcategories,
            { Name: "Transaction" } => Transactions,
            { Name: "Account" } => Accounts,
            { Name: "Counterparty" } => Counterparties,
            { Name: "User" } => Users,
            { Name: "Budget" } => Budgets,
            _ => throw new ArgumentException("Unknown entity type")
        };
    }
}