using Abstractions.DomainBaseTypes;
using Models.Responses;

namespace Identity.Infrastructure.Data;

internal static class DataAccessErrors<TEntity> where TEntity : IEntity
{
    internal static Error AddError = new(
        "DataAccess.Add",
        $"Problem while adding {typeof(TEntity).Name} to the database");
    internal static Error RemoveError = new(
        "DataAccess.Remove",
        $"Problem while removing {typeof(TEntity).Name} to the database");
    internal static Error UpdateError = new(
        "DataAccess.Update",
        $"Problem while updating {typeof(TEntity).Name} to the database");
    internal static Error GetError = new(
        "DataAccess.Update",
        $"Problem while getting {typeof(TEntity).Name}");
    internal static Error NotFound = new(
        "DataAccess.NotFound",
        $"{typeof(TEntity).Name} not found");
}