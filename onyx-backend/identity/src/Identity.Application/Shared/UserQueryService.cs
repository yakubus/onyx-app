using Identity.Application.Errors;
using Identity.Domain;
using Models.Responses;

namespace Identity.Application.Shared;

internal sealed class UserQueryService
{
    private readonly IUserRepository _userRepository;

    internal UserQueryService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    internal async Task<Result<User>> GetUser(Guid? id, string? email, CancellationToken cancellationToken) =>
        (id is not null, email is not null) switch
        {
            (true, false) => await GetByIdAsync(id!.Value, cancellationToken),
            (false, true) => await GetByEmailAsync(email!, cancellationToken),
            _ => Result.Failure<User>(BusinessErrors.InvalidUserQueryRequest)
        };

    private async Task<Result<User>> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var userId = new UserId(id);

        return await _userRepository.GetByIdAsync(userId, cancellationToken);
    }

    private async Task<Result<User>> GetByEmailAsync(string email, CancellationToken cancellationToken)
    {
        var emailCreateresult = Email.Create(email);

        if (emailCreateresult.IsFailure)
        {
            return Result.Failure<User>(emailCreateresult.Error);
        }

        return await _userRepository.GetByEmailAsync(emailCreateresult.Value, cancellationToken);
    }
}