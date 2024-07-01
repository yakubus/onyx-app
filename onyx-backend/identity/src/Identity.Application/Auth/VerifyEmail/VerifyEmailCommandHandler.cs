using Abstractions.Messaging;
using Identity.Application.Abstractions.Authentication;
using Identity.Application.Contracts.Models;
using Identity.Domain;
using Models.Responses;

namespace Identity.Application.Auth.VerifyEmail;

internal sealed class VerifyEmailCommandHandler : ICommandHandler<VerifyEmailCommand, AuthorizationToken>
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtService _jwtService;

    public VerifyEmailCommandHandler(IUserRepository userRepository, IJwtService jwtService)
    {
        _userRepository = userRepository;
        _jwtService = jwtService;
    }

    public async Task<Result<AuthorizationToken>> Handle(VerifyEmailCommand request, CancellationToken cancellationToken)
    {
        var emailCreateResult = Email.Create(request.Email);

        if (emailCreateResult.IsFailure)
        {
            return emailCreateResult.Error;
        }

        var userGetResult = await _userRepository.GetByEmailAsync(emailCreateResult.Value, cancellationToken);

        if (userGetResult.IsFailure)
        {
            return userGetResult.Error;
        }

        var user = userGetResult.Value;

        var longLivedTokenCreateResult = _jwtService.GenerateLongLivedToken();

        if (longLivedTokenCreateResult.IsFailure)
        {
            return longLivedTokenCreateResult.Error;
        }

        var verificationResult = user.VerifyEmail(request.VerificationCode, longLivedTokenCreateResult.Value);

        if (verificationResult.IsFailure)
        {
            return verificationResult.Error;
        }

        var tokenCreateResult = _jwtService.GenerateJwt(user);

        if (tokenCreateResult.IsFailure)
        {
            return tokenCreateResult.Error;
        }

        return new AuthorizationToken(tokenCreateResult.Value, user.LongLivedToken);
    }
}