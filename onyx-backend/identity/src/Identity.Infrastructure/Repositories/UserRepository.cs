using Identity.Domain;
using Models.Responses;
using SharedDAL;

namespace Identity.Infrastructure.Repositories;

internal sealed class UserRepository : Repository<User, UserId>, IUserRepository
{
    public UserRepository(CosmosDbContext context) : base(context)
    {
    }

    public async Task<Result<User>> GetByEmailAsync(
        Domain.Email email,
        CancellationToken cancellationToken) =>
        await GetSingleAsync(
            u => u.Email == email,
            cancellationToken);
}