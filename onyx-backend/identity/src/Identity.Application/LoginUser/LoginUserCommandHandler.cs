using Abstractions.Messaging;
using Identity.Application.Abstractions.Authentication;
using Identity.Application.Errors;
using Identity.Application.Models;
using Identity.Application.Shared;
using Identity.Domain;
using Models.Responses;

namespace Identity.Application.LoginUser;

//TODO implement event grid, to reset LoginAttempts after given time
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
        if (string.IsNullOrWhiteSpace(request.Email))
        {
            return Result.Failure<AuthorizationToken>(BusinessErrors.InvalidUserQueryRequest);
        }

        var userGetResult = await _userQueryService.GetUser(
            null, 
            request.Email,
            cancellationToken);

        if (userGetResult.IsFailure)
        {
            return Result.Failure<AuthorizationToken>(userGetResult.Error);
        }

        var user = userGetResult.Value;

        var loginResult = user.LogIn(request.Password);

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

        return new AuthorizationToken(jwt);
    }
}