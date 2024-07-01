using Abstractions.Messaging;
using Identity.Application.Abstractions.Authentication;
using Identity.Application.Abstractions.UserEvents;
using Identity.Domain;
using Models.Responses;

namespace Identity.Application.User.RemoveUser;

internal sealed class RemoveUserCommandHandler : ICommandHandler<RemoveUserCommand>
{
    private readonly IUserRepository _userRepository;
    private readonly IUserEventsService _userEventsService;
    private readonly IUserContext _userContext;

    public RemoveUserCommandHandler(IUserRepository userRepository, IUserEventsService userEventsService, IUserContext userContext)
    {
        _userRepository = userRepository;
        _userEventsService = userEventsService;
        _userContext = userContext;
    }

    public async Task<Result> Handle(RemoveUserCommand request, CancellationToken cancellationToken)
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
            return userGetResult.Error;
        }

        var user = userGetResult.Value;

        var preRemoveResult = user.Remove(request.Password);

        if (preRemoveResult.IsFailure)
        {
            return preRemoveResult.Error;
        }

        var publishResult = await _userEventsService.PublishUserRemovedEvent(user.Id.Value, cancellationToken);

        if (publishResult.IsFailure)
        {
            return publishResult.Error;
        }

        var removeResult = await _userRepository.RemoveAsync(user.Id, cancellationToken);

        if (removeResult.IsFailure)
        {
            return removeResult.Error;
        }

        return Result.Success();
    }
}