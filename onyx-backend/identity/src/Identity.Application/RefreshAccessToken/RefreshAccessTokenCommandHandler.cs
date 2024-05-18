using Abstractions.Messaging;
using Identity.Application.Abstractions.Authentication;
using Identity.Application.Errors;
using Identity.Application.Models;
using Identity.Domain;
using Models.Responses;

namespace Identity.Application.RefreshAccessToken;

internal sealed class RefreshAccessTokenCommandHandler : IQueryHandler<RefreshAccessTokenCommand, AuthorizationToken>
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtService _jwtService;

    public RefreshAccessTokenCommandHandler(IUserRepository userRepository, IJwtService jwtService)
    {
        _userRepository = userRepository;
        _jwtService = jwtService;
    }

    public async Task<Result<AuthorizationToken>> Handle(RefreshAccessTokenCommand request, CancellationToken cancellationToken)
    {
        var userIdGetResult = _jwtService.GetUserIdFromString(request.Token);

        if (userIdGetResult.IsFailure)
        {
            return Result.Failure<AuthorizationToken>(userIdGetResult.Error);
        }

        var userId = new UserId(userIdGetResult.Value);

        var userGetResult = await _userRepository.GetByIdAsync(userId, cancellationToken);

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