using Identity.Application.Abstractions.UserEvents;
using Models.Responses;

namespace Identity.Infrastructure.EventServices;

internal class UserEventsService : IUserEventsService
{
    //TODO: Implement this service
    public async Task<Result> PublishUserRemovedEvent(Guid userId, CancellationToken cancellationToken)
    {
        return Result.Success();
    }
}