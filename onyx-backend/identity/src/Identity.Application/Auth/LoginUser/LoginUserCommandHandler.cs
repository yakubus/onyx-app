using Abstractions.Messaging;
using Identity.Application.Abstractions.Authentication;
using Identity.Application.Contracts.Models;
using Identity.Application.Contracts.Shared;
using Identity.Domain;
using Models.Responses;

namespace Identity.Application.Auth.LoginUser;

internal sealed class LoginUserCommandHandler : ICommandHandler<LoginUserCommand, AuthorizationToken>
{
    private readonly IJwtService _jwtService;
    private readonly UserQueryService _userQueryService;

    public LoginUserCommandHandler(IJwtService jwtService, IUserRepository userRepository)
    {
        _jwtService = jwtService;
        _userQueryService = new UserQueryService(userRepository);
    }

    public async Task<Result<AuthorizationToken>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var userGetResult = await _userQueryService.GetUser(
            null,
            request.Email,
            cancellationToken);

        if (userGetResult.IsFailure)
        {
            return Result.Failure<AuthorizationToken>(userGetResult.Error);
        }

        var user = userGetResult.Value;

        var longLivedTokenCreateResult = _jwtService.GenerateLongLivedToken();

        if (longLivedTokenCreateResult.IsFailure)
        {
            return longLivedTokenCreateResult.Error;
        }

        var loginResult = user.LogIn(request.Password, longLivedTokenCreateResult.Value);

        if (loginResult.IsFailure)
        {
            return Result.Failure<AuthorizationToken>(loginResult.Error);
        }

        var jwtGenerateResult = _jwtService.GenerateJwt(user);

        if (jwtGenerateResult.IsFailure)
        {
            return Result.Failure<AuthorizationToken>(jwtGenerateResult.Error);
        }

        var jwt = jwtGenerateResult.Value;

        return new AuthorizationToken(jwt, user.LongLivedToken);
    }
}