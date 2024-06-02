using Abstractions.Messaging;
using Identity.Application.Abstractions.Authentication;
using Identity.Application.Messaging.Emails;
using Identity.Domain;
using Models.Responses;

namespace Identity.Application.RequestEmailChange;

internal sealed class RequestEmailChangeCommandHandler : ICommandHandler<RequestEmailChangeCommand>
{
    private readonly IJwtService _jwtService;
    private readonly IEmailService _emailService;
    private readonly IUserRepository _userRepository;

    public RequestEmailChangeCommandHandler(IJwtService jwtService, IUserRepository userRepository, IEmailService emailService)
    {
        _jwtService = jwtService;
        _userRepository = userRepository;
        _emailService = emailService;
    }

    public async Task<Result> Handle(RequestEmailChangeCommand request, CancellationToken cancellationToken)
    {
        var userIdGetResult = _jwtService.GetUserIdFromToken(request.Token);

        if (userIdGetResult.IsFailure)
        {
            return userIdGetResult.Error;
        }

        var userId = new UserId(userIdGetResult.Value);
        var userGetResult = await _userRepository.GetByIdAsync(userId, cancellationToken);

        if (userGetResult.IsFailure)
        {
            return userGetResult.Error;
        }

        var user = userGetResult.Value;

        var requestResult = user.RequestEmailChange();

        if (requestResult.IsFailure)
        {
            return requestResult.Error;
        }

        var verificationCode = requestResult.Value;

        var emailWriter = new EmailWriter(user.Email.Value);

        await _emailService.SendEmailAsync(
            emailWriter.WriteVerificationCode(verificationCode.Code),
            cancellationToken);

        var userUpdateResult = await _userRepository.UpdateAsync(user, cancellationToken);

        if (userUpdateResult.IsFailure)
        {
            return userUpdateResult.Error;
        }

        return Result.Success();
    }
}