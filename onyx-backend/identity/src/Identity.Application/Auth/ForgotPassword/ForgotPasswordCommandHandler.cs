using Abstractions.Messaging;
using Identity.Application.Contracts.Messaging.Emails;
using Identity.Domain;
using Models.Responses;

namespace Identity.Application.Auth.ForgotPassword;

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

        var forgotPasswordRequestResult = user.ForgotPassword();

        if (forgotPasswordRequestResult.IsFailure)
        {
            return forgotPasswordRequestResult.Error;
        }

        var verificationCode = forgotPasswordRequestResult.Value;

        var emailWriter = new EmailWriter(user.Email.Value, user.Username.Value);

        await _emailService.SendEmailAsync(
            emailWriter.WriteForgotPassword(verificationCode.Code),
            cancellationToken);

        var userUpdateResult = await _userRepository.UpdateAsync(user, cancellationToken);

        if (userUpdateResult.IsFailure)
        {
            return userUpdateResult.Error;
        }

        return Result.Success();
    }
}