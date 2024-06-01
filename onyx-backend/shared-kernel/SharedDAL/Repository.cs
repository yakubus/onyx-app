using System.Linq.Expressions;
using Abstractions.DomainBaseTypes;
using Amazon.DynamoDBv2.DocumentModel;
using Models.Responses;

namespace SharedDAL;

public abstract class Repository<TEntity, TEntityId>
    where TEntity : Entity<TEntityId>
    where TEntityId : EntityId, new()
{
    protected readonly Table Table;

    protected Repository(DbContext context)
    {
        Table = context.Set<TEntity>();
    }

    public virtual async Task<Result<IEnumerable<TEntity>>> GetAllAsync(CancellationToken cancellationToken)
    {
        return new List<TEntity>();
    }
        //Result.Create(await Task.Run(
        //    () => Table.GetItemLinqQueryable<TEntity>(true).Where(_ => true).AsEnumerable(),
        //    cancellationToken));

    public async Task<Result<TEntity>> GetByIdAsync(
        TEntityId id,
        CancellationToken cancellationToken = default)
    {
        return null;

        //var response = await Table.ReadItemAsync<TEntity>(
        //    id.Value.ToString(),
        //    new PartitionKey(id.Value.ToString()),
        //    null,
        //    cancellationToken);

        //var entity = response.Resource;

        //return entity is null ?
        //    Result.Failure<TEntity>(DataAccessErrors<TEntity>.NotFound) :
        //    Result.Success(entity);
    }

    public virtual Result<IEnumerable<TEntity>> GetWhere(
        Expression<Func<TEntity, bool>> filterPredicate,
        CancellationToken cancellationToken = default)
    {
        //var queryable = Table.GetItemLinqQueryable<TEntity>(true);
        //var filteredQueryable = queryable.Where(filterPredicate);
        //var result = filteredQueryable.ToList();

        //return result;
        return null;
    }

    public async Task<Result<IEnumerable<TEntity>>> GetManyByIdAsync(
        IEnumerable<TEntityId> ids,
        CancellationToken cancellationToken = default)
    {
        //var entityIds = ids.ToArray();
        //var query = entityIds.Select(
        //    id => (id.Value.ToString(), new PartitionKey(id.Value.ToString())))
        //    .ToList()
        //    .AsReadOnly();

        //var response = await Table.ReadManyItemsAsync<TEntity>(
        //    query,
        //    null,
        //    cancellationToken);

        //var entities = response.Resource;

        //return Result.Create(entities);
        return null;
    }

    public virtual Result<TEntity> GetFirst(
        Expression<Func<TEntity, bool>> filterPredicate,
        CancellationToken cancellationToken = default)
    {
        //var entities = Table.GetItemLinqQueryable<TEntity>(true).Where(filterPredicate).AsEnumerable();

        //var entity = entities.FirstOrDefault();

        //return entity is null ?
        //    Result.Failure<TEntity>(DataAccessErrors<TEntity>.NotFound) :
        //    Result.Success(entity);

        return null;
    }

    public async Task<Result<TEntity>> AddAsync(
        TEntity entity, 
        CancellationToken cancellationToken = default)
    {
        //var response = await Table.CreateItemAsync(
        //    entity,
        //    new(entity.Id.Value.ToString()),
        //    null,
        //    cancellationToken);

        //return Result.Create(response.Resource);

        return null;
    }

    public async Task<Result> AddRangeAsync(
        IEnumerable<TEntity> entities,
        CancellationToken cancellationToken = default)
    {
        //var tasks = entities.Select(
        //    entity => Table.CreateItemAsync(
        //        entity,
        //        new PartitionKey(entity.Id.Value.ToString()),
        //        null,
        //        cancellationToken));

        //await Task.WhenAll(tasks);

        //return Result.Success();
        return null;
    }

    public async Task<Result> RemoveAsync(
        TEntityId entityId, 
        CancellationToken cancellationToken = default)
    {
        //await Table.DeleteItemAsync<TEntity>(
        //    entityId.Value.ToString(),
        //    new PartitionKey(entityId.Value.ToString()),
        //    null,
        //    cancellationToken);

        //return Result.Success();
        return null;
    }

    public async Task<Result> RemoveRangeAsync(
        IEnumerable<TEntityId> entitiesId,
        CancellationToken cancellationToken = default)
    {
        //var tasks = entitiesId.Select(
        //    id => Table.DeleteItemAsync<TEntity>(
        //        id.Value.ToString(),
        //        new PartitionKey(id.Value.ToString()),
        //        null,
        //        cancellationToken));

        //await Task.WhenAll(tasks);

        //return Result.Success();
        return null;
    }

    public async Task<Result<TEntity>> UpdateAsync(
        TEntity entity, 
        CancellationToken cancellationToken = default)
    {
        //var response = await Table.ReplaceItemAsync(
        //    entity,
        //    entity.Id.Value.ToString(),
        //    new PartitionKey(entity.Id.Value.ToString()),
        //    null,
        //    cancellationToken);

        //return Result.Success(response.Resource);
        return null;
    }

    public async Task<Result> UpdateRangeAsync(
        IEnumerable<TEntity> entities,
        CancellationToken cancellationToken = default)
    {
        //var tasks = entities.Select(
        //    entity => Table.ReplaceItemAsync(
        //        entity,
        //        entity.Id.Value.ToString(),
        //        new PartitionKey(entity.Id.Value.ToString()),
        //        null,
        //        cancellationToken));

        //await Task.WhenAll(tasks);

        //return Result.Success();
        return null;
    }
}