using Amazon.DynamoDBv2.DocumentModel;
using Identity.Domain;
using Identity.Infrastructure.Data.DataModels;
using SharedDAL.DataModels.Abstractions;

namespace Identity.Infrastructure.Data.Services;

internal sealed class DataModelService<TEntity> : IDataModelService<TEntity>
{
    private readonly InvalidCastException _convertDomainModelToDataModelException =
        new ($"Cannot cast {typeof(TEntity).Name} to data model");
    private readonly InvalidCastException _convertDocumentToDataModelException = 
        new ($"Cannot cast document to data model of {typeof(TEntity).Name}");

    public IDataModel<TEntity> ConvertDomainModelToDataModel(TEntity entity) =>
        entity switch
        {
            User account => UserDataModel.FromDomainModel(account) as IDataModel<TEntity>,
            _ => throw _convertDomainModelToDataModelException
        } ??
        throw _convertDomainModelToDataModelException;

    public IDataModel<TEntity> ConvertDocumentToDataModel(Document doc) =>
        typeof(TEntity) switch
        {
            var type when type == typeof(User) => UserDataModel.FromDocument(doc) as IDataModel<TEntity>,
            _ => throw _convertDocumentToDataModelException
        } ??
        throw _convertDocumentToDataModelException;
}