using Amazon.DynamoDBv2.DocumentModel;

namespace SharedDAL.DataModels.Abstractions;

public interface IDataModelService<TEntity>
{
    IDataModel<TEntity> ConvertDomainModelToDataModel(TEntity entity);
    IDataModel<TEntity> ConvertDocumentToDataModel(Document doc);
}