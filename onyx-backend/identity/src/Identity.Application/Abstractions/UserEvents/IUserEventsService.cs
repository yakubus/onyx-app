using Models.Responses;

namespace Identity.Application.Abstractions.UserEvents;

public interface IUserEventsService
{
    Task<Result> PublishUserRemovedEvent(Guid userId, CancellationToken cancellationToken);
}