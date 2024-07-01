using Abstractions.DomainBaseTypes;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;

namespace SharedDAL;

public sealed class DbContext
{
    private readonly AmazonDynamoDBClient _client = new();
    public AmazonDynamoDBClient Client => _client;

    private readonly AmazonDynamoDBException _connectionException =
        new ("Could not connect to DynamoDB");
    private AmazonDynamoDBException InvalidTableException(string typeName) =>
        new ($"Table for {typeName} does not exist");

    private Table Users => Table.TryLoadTable(_client, nameof(Users), out var table) ?
        table : throw _connectionException;
    private Table Budgets => Table.TryLoadTable(_client, nameof(Budgets), out var table) ?
        table : throw _connectionException;
    private Table Accounts => Table.TryLoadTable(_client, nameof(Accounts), out var table) ?
        table : throw _connectionException;
    private Table Counterparties => Table.TryLoadTable(_client, nameof(Counterparties), out var table) ?
        table : throw _connectionException;
    private Table Categories => Table.TryLoadTable(_client, nameof(Categories), out var table) ?
        table : throw _connectionException;
    private Table Subcategories => Table.TryLoadTable(_client, nameof(Subcategories), out var table) ?
        table : throw _connectionException;
    private Table Transactions => Table.TryLoadTable(_client, nameof(Transactions), out var table) ?
        table : throw _connectionException;

    public Table Set<TEntity>() where TEntity : IEntity =>
        typeof(TEntity) switch
        {
            { Name: "User" } => Users,
            { Name: "Budget" } => Budgets,
            { Name: "Account" } => Accounts,
            { Name: "Counterparty" } => Counterparties,
            { Name: "Category" } => Categories,
            { Name: "Subcategory" } => Subcategories,
            { Name: "Transaction" } => Transactions,
            var type => throw InvalidTableException(type.Name)
        };
}