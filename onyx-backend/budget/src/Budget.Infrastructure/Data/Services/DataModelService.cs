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
    public IDataModel<TEntity>? ConvertDomainModelToDataModel(TEntity entity) =>
        entity switch
        {
            Account account => AccountDataModel.FromDomainModel(account) as IDataModel<TEntity>,
            Domain.Budgets.Budget budget => BudgetDataModel.FromDomainModel(budget) as IDataModel<TEntity>,
            Category category => CategoryDataModel.FromDomainModel(category) as IDataModel<TEntity>,
            Counterparty counterparty => CounterpartyDataModel.FromDomainModel(counterparty) as IDataModel<TEntity>,
            Subcategory subcategory => SubcategoryDataModel.FromDomainModel(subcategory) as IDataModel<TEntity>,
            Transaction transaction => TransactionDataModel.FromDomainModel(transaction) as IDataModel<TEntity>,
            _ => throw new InvalidCastException($"Cannot cast {typeof(TEntity).Name} to data model")
        };
}