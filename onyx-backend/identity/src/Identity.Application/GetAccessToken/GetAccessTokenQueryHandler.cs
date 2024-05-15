using Abstractions.Messaging;
using Identity.Application.Abstractions.Authentication;
using Identity.Application.Errors;
using Identity.Application.Models;
using Identity.Application.Shared;
using Identity.Domain;
using Models.Responses;

namespace Identity.Application.GetAccessToken;

internal sealed class GetAccessTokenQueryHandler : IQueryHandler<GetAccessTokenQuery, AuthorizationToken>
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtService _jwtService;

    public GetAccessTokenQueryHandler(IUserRepository userRepository, IJwtService jwtService)
    {
        _userRepository = userRepository;
        _jwtService = jwtService;
    }

    public async Task<Result<AuthorizationToken>> Handle(GetAccessTokenQuery request, CancellationToken cancellationToken)
    {
        if (request.UserId is null && request.Email is null && request.Username is null)
        {
            return Result.Failure<AuthorizationToken>(BusinessErrors.InvalidUserQueryRequest);
        }

        var userGetService = new UserQueryService(_userRepository);

        var userGetResult = await userGetService.GetUser(
            request.UserId,
            request.Email,
            request.Username,
            cancellationToken);

        if (userGetResult.IsFailure)
        {
            return Result.Failure<AuthorizationToken>(userGetResult.Error);
        }

        var user = userGetResult.Value;

        if (!user.IsAuthenticated)
        {
            return Result.Failure<AuthorizationToken>(BusinessErrors.UserIsLoggedOut);
        }

        var accessTokenGenerateResult = _jwtService.GenerateJwt(user);

        if (accessTokenGenerateResult.IsFailure)
        {
            return Result.Failure<AuthorizationToken>(accessTokenGenerateResult.Error);
        }

        var accessToken = accessTokenGenerateResult.Value;

        return new AuthorizationToken(accessToken);
    }
}