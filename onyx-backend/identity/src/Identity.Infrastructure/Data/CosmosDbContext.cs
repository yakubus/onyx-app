using System.Net;
using Abstractions.DomainBaseTypes;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;
using User = Identity.Domain.User;

namespace Identity.Infrastructure.Data;

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

    private Container Users => _database.GetContainer(nameof(Users)) 
                                  ?? throw _connectionException;

    internal Container Set<T>() where T : IEntity
    {
        return typeof(T) switch
        {
            var type when type == typeof(User) => Users,
            _ => throw new ArgumentException("Unknown entity type")
        };
    }
}