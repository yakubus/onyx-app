using Models.Responses;

namespace Budget.Domain.Users;

public interface IUserRepository
{
    Task<Result<User>> GetByEmailAsync(string email, CancellationToken cancellationToken);

    Task<Result<User>> GetByIdAsync(UserId userId, CancellationToken cancellationToken);

    Task<Result<User>> AddAsync(User user, CancellationToken cancellationToken);
}