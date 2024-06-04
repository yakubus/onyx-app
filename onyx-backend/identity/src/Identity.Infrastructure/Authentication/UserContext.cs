using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Identity.Application.Abstractions.Authentication;
using Microsoft.AspNetCore.Http;
using Models.Responses;

namespace Identity.Infrastructure.Authentication;

internal sealed class UserContext : IUserContext
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
            .HttpContext?
            .User
            .Claims
            .FirstOrDefault(claim => claim.Type == userIdClaimName)?
            .Value is var id && !string.IsNullOrEmpty(id) ?
            id :
            _userIdClaimNotFound;
}