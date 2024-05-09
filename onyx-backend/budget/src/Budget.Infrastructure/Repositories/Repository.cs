using System.Linq.Expressions;
using Abstractions.DomainBaseTypes;
using Budget.Infrastructure.Data;
using Microsoft.Azure.Cosmos;
using Models.Responses;

namespace Budget.Infrastructure.Repositories;

// TODO Add fetching only records for current budget
internal abstract class Repository<TEntity, TEntityId>
    where TEntity : Entity<TEntityId>
    where TEntityId : EntityId
{
    private readonly CosmosDbContext _context;
    protected readonly Container Container;

    protected Repository(CosmosDbContext context)
    {
        _context = context;
        Container = context.Set<TEntity>();
    }

    public async Task<Result<IEnumerable<TEntity>>> GetAllAsync(CancellationToken cancellationToken) =>
        Result.Create(await Task.Run(
            () => Container.GetItemLinqQueryable<TEntity>(true).Where(_ => true).AsEnumerable(),
            cancellationToken));

    public async Task<Result<TEntity>> GetByIdAsync(
        TEntityId id,
        CancellationToken cancellationToken = default)
    {
        var response = await Container.ReadItemAsync<TEntity>(
            id.Value.ToString(),
            new PartitionKey(id.Value.ToString()),
            null,
            cancellationToken);

        var isSuccess = (int)response.StatusCode >= 200 && (int)response.StatusCode < 300;

        if (!isSuccess)
        {
            return Result.Failure<TEntity>(DataAccessErrors<TEntity>.GetError);
        }

        var entity = response.Resource;

        return entity is null ?
            Result.Failure<TEntity>(DataAccessErrors<TEntity>.NotFound) :
            Result.Success(entity);
    }

    public async Task<Result<IEnumerable<TEntity>>> GetWhereAsync(
        Expression<Func<TEntity, bool>> filterPredicate,
        CancellationToken cancellationToken = default) =>
        Result.Create(await Task.Run(
            () => Container.GetItemLinqQueryable<TEntity>(true).Where(filterPredicate).AsEnumerable(),
            cancellationToken));

    public async Task<Result<TEntity>> GetSingleAsync(
        Expression<Func<TEntity, bool>> filterPredicate,
        CancellationToken cancellationToken = default)
    { 
        var entities = await Task.Run(
            () => Container.GetItemLinqQueryable<TEntity>(true).Where(filterPredicate).AsEnumerable(),
            cancellationToken);

        var entity =  entities.SingleOrDefault();

        return entity is null ?
            Result.Failure<TEntity>(DataAccessErrors<TEntity>.NotFound) :
            Result.Success(entity);
    }

    public async Task<Result<TEntity>> AddAsync(
        TEntity entity, 
        CancellationToken cancellationToken = default)
    {
        var response = await Container.CreateItemAsync(entity, null, null, cancellationToken);

        var isSuccess = (int)response.StatusCode >= 200 && (int)response.StatusCode < 300;

        return isSuccess ?
            Result.Create(response.Resource) :
            Result.Failure<TEntity>(DataAccessErrors<TEntity>.AddError);
    }

    public async Task<Result> AddRangeAsync(
        IEnumerable<TEntity> entities,
        CancellationToken cancellationToken = default)
    {
        var tasks = entities.Select(
            entity => Container.CreateItemAsync(entity, null, null, cancellationToken));

        var responses = await Task.WhenAll(tasks);

        var isSuccess = responses.FirstOrDefault(
            response => (int)response.StatusCode >= 200 && (int)response.StatusCode < 300) is null;

        return isSuccess ?
            Result.Success() :
            Result.Failure<TEntity>(DataAccessErrors<TEntity>.AddError);
    }

    public async Task<Result> RemoveAsync(
        TEntityId entityId, 
        CancellationToken cancellationToken = default)
    {
        var response = await Container.DeleteItemAsync<TEntity>(
            entityId.Value.ToString(),
            new PartitionKey(),
            null,
            cancellationToken);

        var isSuccess = (int)response.StatusCode >= 200 && (int)response.StatusCode < 300;

        return isSuccess ?
            Result.Success() :
            Result.Failure<TEntity>(DataAccessErrors<TEntity>.AddError);
    }

    public async Task<Result> RemoveRangeAsync(
        IEnumerable<TEntity> entities,
        CancellationToken cancellationToken = default)
    {
        var tasks = entities.Select(
            entity => Container.DeleteItemAsync<TEntity>(
                entity.Id.Value.ToString(),
                new PartitionKey(),
                null,
                cancellationToken));

        var responses = await Task.WhenAll(tasks);

        var isSuccess = responses.FirstOrDefault(
            response => (int)response.StatusCode >= 200 && (int)response.StatusCode < 300) is null;

        return isSuccess ?
            Result.Success() :
            Result.Failure<TEntity>(DataAccessErrors<TEntity>.AddError);
    }

    public async Task<Result<TEntity>> UpdateAsync(
        TEntity entity, 
        CancellationToken cancellationToken = default)
    {
        var response = await Container.ReplaceItemAsync(
            entity,
            entity.Id.Value.ToString(),
            null,
            null,
            cancellationToken);

        var isSuccess = (int)response.StatusCode >= 200 && (int)response.StatusCode < 300;

        return isSuccess ?
            Result.Success(response.Resource) :
            Result.Failure<TEntity>(DataAccessErrors<TEntity>.AddError);
    }

    public async Task<Result> UpdateRangeAsync(
        IEnumerable<TEntity> entities,
        CancellationToken cancellationToken = default)
    {
        var tasks = entities.Select(
            entity => Container.ReplaceItemAsync(
                entity,
                entity.Id.Value.ToString(),
                null,
                null,
                cancellationToken));

        var responses = await Task.WhenAll(tasks);

        var isSuccess = responses.FirstOrDefault(
            response => (int)response.StatusCode >= 200 && (int)response.StatusCode < 300) is null;

        return isSuccess ?
            Result.Success() :
            Result.Failure<TEntity>(DataAccessErrors<TEntity>.AddError);
    }

}