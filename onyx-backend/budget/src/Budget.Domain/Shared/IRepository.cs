using Abstractions.DomainBaseTypes;
using Models.Responses;

namespace Budget.Domain.Shared;

public interface IRepository<TEntity, TEntityId> 
    where TEntity : Entity<TEntityId> 
    where TEntityId : EntityId
{
    Task<Result> AddAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task<Result> AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

    Task<Result> RemoveAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task<Result> RemoveRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

    Task<Result> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task<Result> UpdateRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

}