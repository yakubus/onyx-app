using System.Security.AccessControl;
using Abstractions.Messaging;
using Identity.Application.Abstractions.UserEvents;
using Identity.Domain;
using Models.Responses;

namespace Identity.Application.RemoveUser;

internal sealed class RemoveUserCommandHandler : ICommandHandler<RemoveUserCommand>
{
    private readonly IUserRepository _userRepository;
    private readonly IUserEventsService _userEventsService;

    public RemoveUserCommandHandler(IUserRepository userRepository, IUserEventsService userEventsService)
    {
        _userRepository = userRepository;
        _userEventsService = userEventsService;
    }

    public async Task<Result> Handle(RemoveUserCommand request, CancellationToken cancellationToken)
    {
        var userGetResult = await _userRepository.GetByIdAsync(new (request.UserId), cancellationToken);

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

        var removeResult = await _userRepository.DeleteAsync(user.Id, cancellationToken);

        if (removeResult.IsFailure)
        {
            return removeResult.Error;
        }

        return Result.Success();
    }
}