using Models.Responses;

namespace Identity.Application.Abstractions.Authentication;

public interface IJwtService
{
    Result<string> GenerateJwt(Domain.User user);
    Result<string> GetUserIdFromToken(string encodedString);

    Result<string> GenerateLongLivedToken();
}