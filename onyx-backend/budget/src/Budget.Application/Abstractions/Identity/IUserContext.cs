using Models.Responses;

namespace Budget.Application.Abstractions.Identity;

public interface IUserContext
{
    Result<string> GetUserId();

    Result<string> GetUserCurrency();
}