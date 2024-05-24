using Models.Responses;

namespace Identity.Domain;

public interface IUserRepository
{
    Task<Result<User>> GetByIdAsync(UserId id, CancellationToken cancellationToken);
    Task<Result<User>> GetByEmailAsync(Email email, CancellationToken cancellationToken);
    Task<Result<User>> AddAsync(User user, CancellationToken cancellationToken);
    Task<Result<User>> UpdateAsync(User user, CancellationToken cancellationToken);
    Task<Result> RemoveAsync(UserId id, CancellationToken cancellationToken);
}