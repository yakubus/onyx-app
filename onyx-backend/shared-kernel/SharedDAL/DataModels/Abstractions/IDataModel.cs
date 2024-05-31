namespace SharedDAL.DataModels.Abstractions;

public interface IDataModel<out TDomainModel>
{
    Type GetDomainModelType();
    TDomainModel ToDomainModel();
}