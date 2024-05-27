using Budget.Application.Abstractions.Identity;
using Microsoft.AspNetCore.Http;
using Models.Responses;

namespace Budget.Infrastructure.Identity;

//TODO change to internal
public sealed class UserContext : IUserContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private const string userIdClaimName = "Id";
    private const string userCurrencyClaimName = "Currency";
    private readonly Error _userIdClaimNotFound = new(
        "UserContext.UserIdNotFound",
        "Cannot retrieve user ID");
    private readonly Error _usercurrencyClaimNotFound = new(
        "UserContext.CurrencyNotFound",
        "Cannot retrieve base currency for user");

    public UserContext(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Result<string> GetUserId() =>
        _httpContextAccessor
            .HttpContext
            .User
            .Claims
            .FirstOrDefault(claim => claim.Type == userIdClaimName)?
            .Value is var id && !string.IsNullOrEmpty(id) ?
            id : 
            _userIdClaimNotFound;

    public Result<string> GetUserCurrency() =>
        _httpContextAccessor
            .HttpContext
            .User
            .Claims
            .FirstOrDefault(claim => claim.Type == userCurrencyClaimName)?
            .Value is var currency && !string.IsNullOrEmpty(currency) ?
            currency :
            _usercurrencyClaimNotFound;
}