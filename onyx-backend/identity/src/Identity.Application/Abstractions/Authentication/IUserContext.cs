using Models.Responses;

namespace Identity.Application.Abstractions.Authentication;

public interface IUserContext
{
    Result<string> GetUserId();
}