﻿using System.Linq.Expressions;
using Abstractions.DomainBaseTypes;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Models.Responses;
using Newtonsoft.Json;
using SharedDAL.DataModels.Abstractions;
using Expression = Amazon.DynamoDBv2.DocumentModel.Expression;

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

        var records = docs.Select(
            doc => JsonConvert.DeserializeObject<IDataModel<TEntity>>(doc.ToJson()) ??
                   throw new InvalidCastException(
                       $"Cannot convert DynamoDb record to {typeof(IDataModel<TEntity>).Name}"));
        var enitites = records.Select(record => record.ToDomainModel());

        return Result.Create(enitites);
    }

    public async Task<Result<TEntity>> GetByIdAsync(
        TEntityId id,
        CancellationToken cancellationToken = default)
    {
        var doc = await _table.GetItemAsync(new Primitive(id.Value.ToString()), cancellationToken);
        var record = JsonConvert.DeserializeObject<IDataModel<TEntity>>(doc.ToJson());
        
        return record is null ? 
            Result.Failure<TEntity>(DataAccessErrors<TEntity>.NotFound) :
            record.ToDomainModel();
    }

    //TODO Implement
    public virtual Result<IEnumerable<TEntity>> GetWhere(
        Expression<Func<TEntity, bool>> filterPredicate,
        CancellationToken cancellationToken = default)
    {
        var statement = string.Join(
            $"SELECT * FROM {_table.TableName} WHERE ",
            string.Empty);

        _context.Client.ExecuteStatementAsync(
            new ExecuteStatementRequest
            {

            });
        //var queryable = Table.GetItemLinqQueryable<TEntity>(true);
        //var filteredQueryable = queryable.Where(filterPredicate);
        //var result = filteredQueryable.ToList();

        //return result;
        return null;
    }

    //TODO Implement
    public virtual Result<TEntity> GetFirst(
        Expression<Func<TEntity, bool>> filterPredicate,
        CancellationToken cancellationToken = default)
    {
        //var entities = Table.GetItemLinqQueryable<TEntity>(true).Where(filterPredicate).AsEnumerable();

        //var entity = entities.FirstOrDefault();

        //return entity is null ?
        //    Result.Failure<TEntity>(DataAccessErrors<TEntity>.NotFound) :
        //    Result.Success(entity);

        return null;
    }

    public async Task<Result<IEnumerable<TEntity>>> GetManyByIdAsync(
        IEnumerable<TEntityId> ids,
        CancellationToken cancellationToken = default)
    {
        var batch = _table.CreateBatchGet();

        ids.ToList().ForEach(id => batch.AddKey(new Primitive(id.Value.ToString())));

        await batch.ExecuteAsync(cancellationToken);

        var docs = batch.Results;
        var records = docs.Select(
            doc => JsonConvert.DeserializeObject<IDataModel<TEntity>>(doc) ??
                   throw new InvalidCastException(
                       $"Cannot convert DynamoDb record to {typeof(IDataModel<TEntity>).Name}"));
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