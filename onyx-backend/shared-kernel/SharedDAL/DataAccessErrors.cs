using Abstractions.DomainBaseTypes;
using Models.Responses;

namespace SharedDAL;

public static class DataAccessErrors<TEntity> where TEntity : IEntity
{
    public static readonly Error AddError = new(
        "DataAccess.Add",
        $"Problem while adding {typeof(TEntity).Name} to the database");
    public static readonly Error RemoveError = new(
        "DataAccess.Remove",
        $"Problem while removing {typeof(TEntity).Name} to the database");
    public static readonly Error UpdateError = new(
        "DataAccess.Update",
        $"Problem while updating {typeof(TEntity).Name} to the database");
    public static readonly Error GetError = new(
        "DataAccess.Update",
        $"Problem while getting {typeof(TEntity).Name}");
    public static readonly Error NotFound = new(
        "DataAccess.NotFound",
        $"{typeof(TEntity).Name} not found");
    public static readonly Error TransactionError = new(
        "DataAccess.Transaction",
        $"Transaction commit failure");
}