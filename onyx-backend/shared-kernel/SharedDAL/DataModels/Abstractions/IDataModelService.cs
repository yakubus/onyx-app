namespace SharedDAL.DataModels.Abstractions;

public interface IDataModelService<TEntity>
{
    IDataModel<TEntity>? ConvertDomainModelToDataModel(TEntity entity);
}