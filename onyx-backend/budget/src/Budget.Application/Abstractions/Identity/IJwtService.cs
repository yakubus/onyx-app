using Budget.Domain.Users;
using Models.Responses;

namespace Budget.Application.Abstractions.Identity;

public interface IJwtService
{
    Result<string> GenerateJwt(User user);
    Result<string> GetUserIdFromToken(string encodedString);
}