using Abstractions.Messaging;
using Identity.Application.Abstractions.Authentication;
using Identity.Domain;
using Models.Responses;

namespace Identity.Application.User.LogoutUser;

internal sealed class LogoutUserCommandHandler : ICommandHandler<LogoutUserCommand>
{
    private readonly IUserRepository _userRepository;
    private readonly IUserContext _userContext;

    public LogoutUserCommandHandler(IUserRepository userRepository, IUserContext userContext)
    {
        _userRepository = userRepository;
        _userContext = userContext;
    }

    public async Task<Result> Handle(LogoutUserCommand request, CancellationToken cancellationToken)
    {
        var userIdGetResult = _userContext.GetUserId();

        if (userIdGetResult.IsFailure)
        {
            return userIdGetResult.Error;
        }

        var userId = new UserId(userIdGetResult.Value);

        var userGetResult = await _userRepository.GetByIdAsync(userId, cancellationToken);

        if (userGetResult.IsFailure)
        {
            return Result.Failure(userGetResult.Error);
        }

        var user = userGetResult.Value;

        var logoutResult = user.LogOut();

        if (logoutResult.IsFailure)
        {
            return Result.Failure(logoutResult.Error);
        }

        var updateResult = await _userRepository.UpdateAsync(user, cancellationToken);

        if (updateResult.IsFailure)
        {
            return Result.Failure(updateResult.Error);
        }

        return Result.Success();
    }
}