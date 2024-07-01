using Abstractions.Messaging;
using Identity.Application.Contracts.Messaging.Emails;
using Identity.Domain;
using Models.Responses;

namespace Identity.Application.Auth.ResendEmail;

internal sealed class ResendEmailCommandHandler : ICommandHandler<ResendEmailCommand>
{
    private readonly IEmailService _emailService;
    private readonly IUserRepository _userRepository;

    public ResendEmailCommandHandler(IEmailService emailService, IUserRepository userRepository)
    {
        _emailService = emailService;
        _userRepository = userRepository;
    }

    public async Task<Result> Handle(ResendEmailCommand request, CancellationToken cancellationToken)
    {
        var messageTypeCreateResult = MessageType.FromString(request.MessageType);

        if (messageTypeCreateResult.IsFailure)
        {
            return messageTypeCreateResult.Error;
        }

        var messageType = messageTypeCreateResult.Value;

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

        var reGenerateResult = user.ReGenerateVerificationCode();

        if (reGenerateResult.IsFailure)
        {
            return reGenerateResult.Error;
        }

        var verificationCode = reGenerateResult.Value;

        var emailWriter = new EmailWriter(user.Email.Value, user.Username.Value);

        var sendResult = messageType switch
        {
            _ when messageType == MessageType.EmailChangeRequest =>
                await _emailService.SendEmailAsync(
                    emailWriter.WriteChangeEmail(verificationCode.Code),
                    cancellationToken),
            _ when messageType == MessageType.ForgotPassword =>
                await _emailService.SendEmailAsync(
                    emailWriter.WriteForgotPassword(verificationCode.Code),
                    cancellationToken),
            _ when messageType == MessageType.EmailVerification =>
                await _emailService.SendEmailAsync(
                    emailWriter.WriteEmailVerification(verificationCode.Code),
                    cancellationToken),
        };

        if (sendResult.IsFailure)
        {
            return sendResult.Error;
        }

        var userUpdateResult = await _userRepository.UpdateAsync(user, cancellationToken);

        if (userUpdateResult.IsFailure)
        {
            return userUpdateResult.Error;
        }

        return Result.Success();
    }
}