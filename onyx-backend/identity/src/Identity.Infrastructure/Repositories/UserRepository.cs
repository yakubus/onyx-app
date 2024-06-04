using Identity.Domain;
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
        CancellationToken cancellationToken) =>
        await GetFirstAsync(
            $"Email = '{email.Value}'",
            cancellationToken);
}