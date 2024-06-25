using Abstractions.DomainBaseTypes;
using Amazon.DynamoDBv2.DocumentModel;
using Models.Responses;
using Newtonsoft.Json;
using SharedDAL.DataModels.Abstractions;

namespace SharedDAL;

public abstract class Repository<TEntity, TEntityId>
    where TEntity : Entity<TEntityId>
    where TEntityId : EntityId, new()
{
    protected readonly Table Table;
    protected readonly DbContext Context;
    protected readonly IDataModelService<TEntity> DataModelService;

    protected Repository(DbContext context, IDataModelService<TEntity> dataModelService)
    {
        Context = context;
        DataModelService = dataModelService;
        Table = context.Set<TEntity>();
    }

    public virtual async Task<Result<IEnumerable<TEntity>>> GetAllAsync(CancellationToken cancellationToken)
    {
        var config = new ScanOperationConfig
        {
            Select = SelectValues.AllAttributes,
            Filter = new ScanFilter(),
            Limit = 1000
        };

        var scanner = Table.Scan(config);
        var docs = new List<Document>();

        do 
            docs.AddRange(await scanner.GetNextSetAsync(cancellationToken));
        while (!scanner.IsDone);

        var records = docs.Select(DataModelService.ConvertDocumentToDataModel);
        var enitites = records.Select(record => record.ToDomainModel());

        return Result.Create(enitites);
    }

    public async Task<Result<TEntity>> GetByIdAsync(
        TEntityId id,
        CancellationToken cancellationToken = default)
    {
        var doc = await Table.GetItemAsync(new Primitive(id.Value.ToString()), cancellationToken);

        if (doc is null)
        {
            return DataAccessErrors<TEntity>.NotFound;
        }
        
        var record = DataModelService.ConvertDocumentToDataModel(doc);

        return record.ToDomainModel();
    }

    protected virtual async Task<Result<IEnumerable<TEntity>>> GetWhereAsync(
        ScanFilter filter,
        CancellationToken cancellationToken = default)
    {
        var scanner = Table.Scan(filter);

        var docs = new List<Document>();

        do
            docs.AddRange(await scanner.GetNextSetAsync(cancellationToken));
        while (!scanner.IsDone);

        var records = docs.Select(DataModelService.ConvertDocumentToDataModel);
        var enitites = records.Select(record => record.ToDomainModel());

        return Result.Create(enitites);
    }

    protected virtual async Task<Result<TEntity>> GetFirstAsync(
        ScanFilter filter,
        CancellationToken cancellationToken = default)
    {
        var scanner = Table.Scan(filter);

        var docs = await scanner.GetNextSetAsync(cancellationToken);

        var doc = docs.FirstOrDefault();

        if (doc is null)
        {
            return DataAccessErrors<TEntity>.NotFound;
        }

        var record = DataModelService.ConvertDocumentToDataModel(doc);
        var entity = record.ToDomainModel();

        return entity;
    }

    public async Task<Result<IEnumerable<TEntity>>> GetManyByIdAsync(
        IEnumerable<TEntityId> ids,
        CancellationToken cancellationToken = default)
    {
        var batch = Table.CreateBatchGet();

        ids.ToList().ForEach(id => batch.AddKey(new Primitive(id.Value.ToString())));

        await batch.ExecuteAsync(cancellationToken);

        var docs = batch.Results;
        var records = docs.Select(DataModelService.ConvertDocumentToDataModel);
        var enitites = records.Select(record => record.ToDomainModel());

        return Result.Create(enitites);
    }

    public async Task<Result> RemoveAsync(
        TEntityId entityId, 
        CancellationToken cancellationToken = default)
    {
        await Table.DeleteItemAsync(new Primitive(entityId.Value.ToString()), cancellationToken);

        return Result.Success();
    }

    public async Task<Result> RemoveRangeAsync(
        IEnumerable<TEntityId> entitiesId,
        CancellationToken cancellationToken = default)
    {
        var batch = Table.CreateBatchWrite();

        entitiesId.ToList().ForEach(id => batch.AddKeyToDelete(new Primitive(id.Value.ToString())));

        await batch.ExecuteAsync(cancellationToken);

        return Result.Success();
    }

    public async Task<Result<TEntity>> AddAsync(
        TEntity entity,
        CancellationToken cancellationToken = default)
    {
        var record = DataModelService.ConvertDomainModelToDataModel(entity);
        var json = JsonConvert.SerializeObject(record);
        var doc = Document.FromJson(json);

        await Table.PutItemAsync(doc, cancellationToken);

        return Result.Success(entity);
    }

    public async Task<Result> AddRangeAsync(
        IEnumerable<TEntity> entities,
        CancellationToken cancellationToken = default)
    {
        var batch = Table.CreateBatchWrite();

        var records = entities.Select(DataModelService.ConvertDomainModelToDataModel);
        var jsons = records.Select(JsonConvert.SerializeObject);
        var docs = jsons.Select(Document.FromJson);

        docs.ToList().ForEach(batch.AddDocumentToPut);

        await batch.ExecuteAsync(cancellationToken);

        return Result.Success();
    }

    public async Task<Result<TEntity>> UpdateAsync(
        TEntity entity,
        CancellationToken cancellationToken = default)
    {
        return await AddAsync(entity, cancellationToken);
    }

    public async Task<Result> UpdateRangeAsync(
        IEnumerable<TEntity> entities,
        CancellationToken cancellationToken = default)
    {
        return await AddRangeAsync(entities, cancellationToken);
    }
}