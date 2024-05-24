using Abstractions.DomainBaseTypes;
using Models.Responses;

namespace SharedDAL;

public static class DataAccessErrors<TEntity> where TEntity : IEntity
{
    public static Error AddError = new(
        "DataAccess.Add",
        $"Problem while adding {typeof(TEntity).Name} to the database");
    public static Error RemoveError = new(
        "DataAccess.Remove",
        $"Problem while removing {typeof(TEntity).Name} to the database");
    public static Error UpdateError = new(
        "DataAccess.Update",
        $"Problem while updating {typeof(TEntity).Name} to the database");
    public static Error GetError = new(
        "DataAccess.Update",
        $"Problem while getting {typeof(TEntity).Name}");
    public static Error NotFound = new(
        "DataAccess.NotFound",
        $"{typeof(TEntity).Name} not found");
}