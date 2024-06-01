using Identity.Domain;
using Models.Responses;
using SharedDAL;

namespace Identity.Infrastructure.Repositories;

internal sealed class UserRepository : Repository<User, UserId>, IUserRepository
{
    public UserRepository(DbContext context) : base(context)
    {
    }

    public Result<User> GetByEmailAsync(
        Domain.Email email,
        CancellationToken cancellationToken) =>
        GetFirst(
            u => u.Email == email,
            cancellationToken);
}