using Identity.Domain;
using Models.Responses;

namespace Identity.Application.Abstractions.Authentication;

public interface IJwtService
{
    Result<string> GenerateJwt(User user);
    Result<string> GetUserIdFromString(string encodedString);
}