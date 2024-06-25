using Amazon.DynamoDBv2.DocumentModel;
using Identity.Domain;
using Identity.Infrastructure.Data.DataModels;
using Models.Responses;
using SharedDAL;
using SharedDAL.DataModels.Abstractions;

namespace Identity.Infrastructure.Repositories;

internal sealed class UserRepository : Repository<User, UserId>, IUserRepository
{
    public UserRepository(DbContext context, IDataModelService<User> dataModelService) : base(context, dataModelService)
    {
    }

    public async Task<Result<User>> GetByEmailAsync(
        Domain.Email email,
        CancellationToken cancellationToken)
    {
        var filter = new ScanFilter();

        filter.AddCondition(nameof(UserDataModel.Email), ScanOperator.Equal, email.Value);

        return await GetFirstAsync(
            filter,
            cancellationToken);
    }
}