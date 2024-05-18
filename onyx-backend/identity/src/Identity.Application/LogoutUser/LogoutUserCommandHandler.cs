using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abstractions.Messaging;
using Identity.Domain;
using Models.Responses;

namespace Identity.Application.LogoutUser;

internal sealed class LogoutUserCommandHandler : ICommandHandler<LogoutUserCommand>
{
    private readonly IUserRepository _userRepository;

    public LogoutUserCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result> Handle(LogoutUserCommand request, CancellationToken cancellationToken)
    {
        var userId = new UserId(request.UserId);

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