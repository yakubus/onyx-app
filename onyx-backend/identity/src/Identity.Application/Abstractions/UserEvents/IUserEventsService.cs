using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Identity.Domain;
using Models.Responses;

namespace Identity.Application.Abstractions.UserEvents
{
    public interface IUserEventsService
    {
        Task<Result> PublishUserRemovedEvent(Guid userId, CancellationToken cancellationToken);
    }
}
