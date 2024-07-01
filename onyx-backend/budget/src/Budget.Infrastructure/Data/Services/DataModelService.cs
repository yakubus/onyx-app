using Amazon.DynamoDBv2.DocumentModel;
using Budget.Domain.Accounts;
using Budget.Domain.Categories;
using Budget.Domain.Counterparties;
using Budget.Domain.Subcategories;
using Budget.Infrastructure.Data.DataModels.Accounts;
using Budget.Infrastructure.Data.DataModels.Budgets;
using Budget.Infrastructure.Data.DataModels.Categories;
using Budget.Infrastructure.Data.DataModels.Counterparties;
using Budget.Infrastructure.Data.DataModels.Subcategories;
using Budget.Infrastructure.Data.DataModels.Transactions;
using SharedDAL.DataModels.Abstractions;
using Transaction = Budget.Domain.Transactions.Transaction;

namespace Budget.Infrastructure.Data.Services;

internal sealed class DataModelService<TEntity> : IDataModelService<TEntity>
{
    private readonly InvalidCastException _convertDomainModelToDataModelException =
        new ($"Cannot cast {typeof(TEntity).Name} to data model");
    private readonly InvalidCastException _convertDocumentToDataModelException = 
        new ($"Cannot cast document to data model of {typeof(TEntity).Name}");

    public IDataModel<TEntity> ConvertDomainModelToDataModel(TEntity entity) =>
        entity switch
        {
            Account account => AccountDataModel.FromDomainModel(account) as IDataModel<TEntity>,
            Domain.Budgets.Budget budget => BudgetDataModel.FromDomainModel(budget) as IDataModel<TEntity>,
            Category category => CategoryDataModel.FromDomainModel(category) as IDataModel<TEntity>,
            Counterparty counterparty => CounterpartyDataModel.FromDomainModel(counterparty) as IDataModel<TEntity>,
            Subcategory subcategory => SubcategoryDataModel.FromDomainModel(subcategory) as IDataModel<TEntity>,
            Transaction transaction => TransactionDataModel.FromDomainModel(transaction) as IDataModel<TEntity>,
            _ => throw _convertDomainModelToDataModelException
        } ??
        throw _convertDomainModelToDataModelException;

    public IDataModel<TEntity> ConvertDocumentToDataModel(Document doc) =>
        typeof(TEntity) switch
        {
            var type when type == typeof(Account) => AccountDataModel.FromDocument(doc) as IDataModel<TEntity>,
            var type when type == typeof(Account) => BudgetDataModel.FromDocument(doc) as IDataModel<TEntity>,
            var type when type == typeof(Account) => CategoryDataModel.FromDocument(doc) as IDataModel<TEntity>,
            var type when type == typeof(Account) => CounterpartyDataModel.FromDocument(doc) as IDataModel<TEntity>,
            var type when type == typeof(Account) => SubcategoryDataModel.FromDocument(doc) as IDataModel<TEntity>,
            var type when type == typeof(Account) => TransactionDataModel.FromDocument(doc) as IDataModel<TEntity>,
            _ => throw _convertDocumentToDataModelException
        } ??
        throw _convertDocumentToDataModelException;
}