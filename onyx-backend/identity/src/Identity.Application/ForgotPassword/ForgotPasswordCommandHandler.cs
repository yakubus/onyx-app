using Abstractions.Messaging;
using Identity.Application.Messaging.Emails;
using Identity.Domain;
using Models.Responses;

namespace Identity.Application.ForgotPassword;

internal sealed record ForgotPasswordCommandHandler : ICommandHandler<ForgotPasswordCommand>
{
    private readonly IUserRepository _userRepository;
    private readonly IEmailService _emailService;

    public ForgotPasswordCommandHandler(IUserRepository userRepository, IEmailService emailService)
    {
        _userRepository = userRepository;
        _emailService = emailService;
    }

    public async Task<Result> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        var userGetResult = await _userRepository.GetByIdAsync(new(request.UserId), cancellationToken);

        if (userGetResult.IsFailure)
        {
            return userGetResult.Error;
        }

        var user = userGetResult.Value;

        var forgotPasswordRequestResult = user.ForgotPassword();

        if (forgotPasswordRequestResult.IsFailure)
        {
            return forgotPasswordRequestResult.Error;
        }

        var verificationCode = forgotPasswordRequestResult.Value;

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