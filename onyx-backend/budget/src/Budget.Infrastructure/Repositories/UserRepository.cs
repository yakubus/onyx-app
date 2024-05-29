using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Budget.Domain.Users;
using Models.Responses;
using SharedDAL;

namespace Budget.Infrastructure.Repositories;

internal sealed class UserRepository : Repository<User, UserId>, IUserRepository
{
    public UserRepository(CosmosDbContext context) : base(context)
    {
        }

    public async Task<Result<User>> GetByEmailAsync(string email, CancellationToken cancellationToken)
    {
            return await Task.Run(() => GetFirst(u => u.Email == email), cancellationToken);
        }
}