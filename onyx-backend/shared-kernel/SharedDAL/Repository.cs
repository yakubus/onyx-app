using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using Abstractions.DomainBaseTypes;
using Microsoft.Azure.Cosmos;
using Models.Responses;
using MongoDB.Bson.Serialization.Serializers;
using Container = Microsoft.Azure.Cosmos.Container;

namespace SharedDAL;

// TODO Add fetching only records for current budget
public abstract class Repository<TEntity, TEntityId>
    where TEntity : Entity<TEntityId>
    where TEntityId : EntityId, new()
{
    protected readonly Container Container;

    protected Repository(CosmosDbContext context)
    {
        Container = context.Set<TEntity>();
    }

    public virtual async Task<Result<IEnumerable<TEntity>>> GetAllAsync(CancellationToken cancellationToken) =>
        Result.Create(await Task.Run(
            () => Container.GetItemLinqQueryable<TEntity>(true).Where(_ => true).AsEnumerable(),
            cancellationToken));

    public async Task<Result<TEntity>> GetByIdAsync(
        TEntityId id,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await Container.ReadItemAsync<TEntity>(
                id.Value.ToString(),
                new PartitionKey(id.Value.ToString()),
                null,
                cancellationToken);

            var entity = response.Resource;

            return entity is null ?
                Result.Failure<TEntity>(DataAccessErrors<TEntity>.NotFound) :
                Result.Success(entity);
        }
        catch (Exception)
        {
            return Result.Failure<TEntity>(DataAccessErrors<TEntity>.NotFound);
        }
    }

    public virtual Result<IEnumerable<TEntity>> GetWhere(
        Expression<Func<TEntity, bool>> filterPredicate,
        CancellationToken cancellationToken = default)
    {
        var queryable = Container.GetItemLinqQueryable<TEntity>(true);
        var filteredQueryable = queryable.Where(filterPredicate);
        var result = filteredQueryable.ToList();

        return result;
    }

    public async Task<Result<IEnumerable<TEntity>>> GetManyByIdAsync(
        IEnumerable<TEntityId> ids,
        CancellationToken cancellationToken = default)
    {
        var entityIds = ids.ToArray();
        var query = entityIds.Select(
            id => (id.Value.ToString(), new PartitionKey(id.Value.ToString())))
            .ToList()
            .AsReadOnly();

        try
        {
            var response = await Container.ReadManyItemsAsync<TEntity>(
                query,
                null,
                cancellationToken);

            var entities = response.Resource;

            return Result.Create(entities);
        }
        catch (Exception )
        {
            return Result.Failure<IEnumerable<TEntity>>(DataAccessErrors<TEntity>.NotFound);
        }
    }

    public virtual Result<TEntity> GetFirst(
        Expression<Func<TEntity, bool>> filterPredicate,
        CancellationToken cancellationToken = default)
    { 
        var entities = Container.GetItemLinqQueryable<TEntity>(true).Where(filterPredicate).AsEnumerable();

        var entity = entities.FirstOrDefault();

        return entity is null ?
            Result.Failure<TEntity>(DataAccessErrors<TEntity>.NotFound) :
            Result.Success(entity);
    }

    public async Task<Result<TEntity>> AddAsync(
        TEntity entity, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await Container.CreateItemAsync(
                entity,
                new(entity.Id.Value.ToString()),
                null,
                cancellationToken);

            return Result.Create(response.Resource);
        }
        catch (Exception e)
        {
            return Result.Failure<TEntity>(DataAccessErrors<TEntity>.AddError);
        }

    }

    public async Task<Result> AddRangeAsync(
        IEnumerable<TEntity> entities,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var tasks = entities.Select(
                entity => Container.CreateItemAsync(
                    entity,
                    new PartitionKey(entity.Id.Value.ToString()),
                    null,
                    cancellationToken));

            await Task.WhenAll(tasks);

            return Result.Success();
        }
        catch (Exception)
        {
            return Result.Failure<TEntity>(DataAccessErrors<TEntity>.AddError);
        }
    }

    public async Task<Result> RemoveAsync(
        TEntityId entityId, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            await Container.DeleteItemAsync<TEntity>(
                entityId.Value.ToString(),
                new PartitionKey(entityId.Value.ToString()),
                null,
                cancellationToken);

            return Result.Success();
        }
        catch (Exception)
        {
            return Result.Failure(DataAccessErrors<TEntity>.RemoveError);
        }
    }

    public async Task<Result> RemoveRangeAsync(
        IEnumerable<TEntityId> entitiesId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var tasks = entitiesId.Select(
                id => Container.DeleteItemAsync<TEntity>(
                    id.Value.ToString(),
                    new PartitionKey(id.Value.ToString()),
                    null,
                    cancellationToken));

            await Task.WhenAll(tasks);

            return Result.Success();
        }
        catch (Exception)
        {
            return Result.Failure(DataAccessErrors<TEntity>.RemoveError);
        }
    }

    public async Task<Result<TEntity>> UpdateAsync(
        TEntity entity, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await Container.ReplaceItemAsync(
                entity,
                entity.Id.Value.ToString(),
                new PartitionKey(entity.Id.Value.ToString()),
                null,
                cancellationToken);

            return Result.Success(response.Resource);
        }
        catch (Exception)
        {
            return Result.Failure<TEntity>(DataAccessErrors<TEntity>.UpdateError);
        }
    }

    public async Task<Result> UpdateRangeAsync(
        IEnumerable<TEntity> entities,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var tasks = entities.Select(
                entity => Container.ReplaceItemAsync(
                    entity,
                    entity.Id.Value.ToString(),
                    new PartitionKey(entity.Id.Value.ToString()),
                    null,
                    cancellationToken));

            await Task.WhenAll(tasks);

            return Result.Success();
        }
        catch (Exception)
        {
            return Result.Failure<TEntity>(DataAccessErrors<TEntity>.UpdateError);
        }
    }
}