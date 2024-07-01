using Abstractions.Messaging;
using Identity.Application.Abstractions.Authentication;
using Identity.Application.Contracts.Models;
using Identity.Domain;
using Models.Responses;

namespace Identity.Application.Auth.NewPassword;

internal sealed class NewPasswordCommandHandler : ICommandHandler<NewPasswordCommand, AuthorizationToken>
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtService _jwtService;

    public NewPasswordCommandHandler(IUserRepository userRepository, IJwtService jwtService)
    {
        _userRepository = userRepository;
        _jwtService = jwtService;
    }

    public async Task<Result<AuthorizationToken>> Handle(NewPasswordCommand request, CancellationToken cancellationToken)
    {
        var emailCreateResult = Email.Create(request.Email);

        if (emailCreateResult.IsFailure)
        {
            return emailCreateResult.Error;
        }

        var email = emailCreateResult.Value;

        var userGetResult = await _userRepository.GetByEmailAsync(email, cancellationToken);

        if (userGetResult.IsFailure)
        {
            return userGetResult.Error;
        }

        var user = userGetResult.Value;

        var passwordChangeResult = user.ChangePassword(request.NewPassword, request.VerificationCode);

        if (passwordChangeResult.IsFailure)
        {
            return passwordChangeResult.Error;
        }

        var (tokenCreateResult, longLivedTokenCreateResult) = 
            (_jwtService.GenerateJwt(user), _jwtService.GenerateLongLivedToken());

        if (Result.Aggregate(tokenCreateResult, longLivedTokenCreateResult) is var result && result.IsFailure)
        {
            return result.Error;
        }

        var (token, longLivedToken) = (tokenCreateResult.Value, longLivedTokenCreateResult.Value);

        user.SetLongLivedToken(longLivedToken);

        var updateResult = await _userRepository.UpdateAsync(user, cancellationToken);

        if (updateResult.IsFailure)
        {
            return updateResult.Error;
        }

        user = updateResult.Value;

        return new AuthorizationToken(token, longLivedToken);
    }
}