using Abstractions.DomainBaseTypes;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Amazon.Runtime.Internal.Auth;
using Models.Responses;
using Newtonsoft.Json;
using SharedDAL.DataModels.Abstractions;

namespace SharedDAL;

public abstract class Repository<TEntity, TEntityId>
    where TEntity : Entity<TEntityId>
    where TEntityId : EntityId, new()
{
    private readonly Table _table;
    private readonly DbContext _context;
    private readonly IDataModelService<TEntity> _dataModelService;

    protected Repository(DbContext context, IDataModelService<TEntity> dataModelService)
    {
        _context = context;
        _dataModelService = dataModelService;
        _table = context.Set<TEntity>();
    }

    public virtual async Task<Result<IEnumerable<TEntity>>> GetAllAsync(CancellationToken cancellationToken)
    {
        var config = new ScanOperationConfig
        {
            Select = SelectValues.AllAttributes,
            Filter = new ScanFilter(),
            Limit = 1000
        };

        var scanner = _table.Scan(config);
        var docs = new List<Document>();

        do 
            docs.AddRange(await scanner.GetNextSetAsync(cancellationToken));
        while (!scanner.IsDone);

        var records = docs.Select(_dataModelService.ConvertDocumentToDataModel);
        var enitites = records.Select(record => record.ToDomainModel());

        return Result.Create(enitites);
    }

    public async Task<Result<TEntity>> GetByIdAsync(
        TEntityId id,
        CancellationToken cancellationToken = default)
    {
        var doc = await _table.GetItemAsync(new Primitive(id.Value.ToString()), cancellationToken);

        if (doc is null)
        {
            return DataAccessErrors<TEntity>.NotFound;
        }
        
        var record = _dataModelService.ConvertDocumentToDataModel(doc);

        return record.ToDomainModel();
    }

    /// <summary>
    /// Query the database with a custom query
    /// </summary>
    /// <param name="query">Must be a valid DynamoDB query<br/>
    ///     Must contain only logical part of query (without SELECT FROM statement) </param>
    /// <param name="cancellationToken"></param>
    /// <returns>List of TEntity type</returns>
    public virtual async Task<Result<IEnumerable<TEntity>>> GetWhereAsync(
        string query,
        CancellationToken cancellationToken = default)
    {
        var statement = string.Join(
            $"SELECT * FROM {_table.TableName} WHERE ",
            query);

        var response = await _context.Client.ExecuteStatementAsync(
            new ExecuteStatementRequest
            {
                Statement = statement
            },
            cancellationToken);

        var items = response.Items;
        var docs = items.Select(Document.FromAttributeMap);

        var records = docs.Select(_dataModelService.ConvertDocumentToDataModel);
        var enitites = records.Select(record => record.ToDomainModel());

        return Result.Create(enitites);
    }

    /// <summary>
    /// Query the database with a custom query
    /// </summary>
    /// <param name="query">Must be a valid DynamoDB query<br/>
    ///     Must contain only logical part of query (without SELECT FROM statement) </param>
    /// <param name="cancellationToken"></param>
    /// <returns>Object of TEntity type, which is first found<br/>
    /// Failure when no record was found</returns>
    public virtual async Task<Result<TEntity>> GetFirstAsync(
        string query,
        CancellationToken cancellationToken = default)
    {
        var statement = string.Join(
            $"SELECT * FROM {_table.TableName} WHERE ",
            query);

        var response = await _context.Client.ExecuteStatementAsync(
            new ExecuteStatementRequest
            {
                Statement = statement,
            },
            cancellationToken);

        var item = response.Items.FirstOrDefault();

        if (item is null)
        {
            return DataAccessErrors<TEntity>.NotFound;
        }

        var doc = Document.FromAttributeMap(item);

        var record = _dataModelService.ConvertDocumentToDataModel(doc);
        var entity = record.ToDomainModel();

        return entity;
    }

    public async Task<Result<IEnumerable<TEntity>>> GetManyByIdAsync(
        IEnumerable<TEntityId> ids,
        CancellationToken cancellationToken = default)
    {
        var batch = _table.CreateBatchGet();

        ids.ToList().ForEach(id => batch.AddKey(new Primitive(id.Value.ToString())));

        await batch.ExecuteAsync(cancellationToken);

        var docs = batch.Results;
        var records = docs.Select(_dataModelService.ConvertDocumentToDataModel);
        var enitites = records.Select(record => record.ToDomainModel());

        return Result.Create(enitites);
    }

    public async Task<Result> RemoveAsync(
        TEntityId entityId, 
        CancellationToken cancellationToken = default)
    {
        await _table.DeleteItemAsync(new Primitive(entityId.Value.ToString()), cancellationToken);

        return Result.Success();
    }

    public async Task<Result> RemoveRangeAsync(
        IEnumerable<TEntityId> entitiesId,
        CancellationToken cancellationToken = default)
    {
        var batch = _table.CreateBatchWrite();

        entitiesId.ToList().ForEach(id => batch.AddKeyToDelete(new Primitive(id.Value.ToString())));

        await batch.ExecuteAsync(cancellationToken);

        return Result.Success();
    }

    public async Task<Result<TEntity>> AddAsync(
        TEntity entity,
        CancellationToken cancellationToken = default)
    {
        var record = _dataModelService.ConvertDomainModelToDataModel(entity);
        var json = JsonConvert.SerializeObject(record);
        var doc = Document.FromJson(json);

        await _table.PutItemAsync(doc, cancellationToken);

        return Result.Success(entity);
    }

    public async Task<Result> AddRangeAsync(
        IEnumerable<TEntity> entities,
        CancellationToken cancellationToken = default)
    {
        var batch = _table.CreateBatchWrite();

        var records = entities.Select(_dataModelService.ConvertDomainModelToDataModel);
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