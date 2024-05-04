using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abstractions.DomainBaseTypes;
using Models.Responses;

namespace Budget.Domain.Shared;

public interface IRepository<TEntity, TEntityId> 
    where TEntity : Entity<TEntityId> 
    where TEntityId : EntityId
{
    Task<Result> Add(TEntity entity, CancellationToken cancellationToken = default);
    Task<Result> AddRange(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

    Task<Result> Remove(TEntity entity, CancellationToken cancellationToken = default);
    Task<Result> RemoveRange(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

    Task<Result> Update(TEntity entity, CancellationToken cancellationToken = default);
    Task<Result> UpdateRange(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

}