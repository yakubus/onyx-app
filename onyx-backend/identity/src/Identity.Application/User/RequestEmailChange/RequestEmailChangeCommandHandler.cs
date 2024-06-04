using Abstractions.Messaging;
using Identity.Application.Abstractions.Authentication;
using Identity.Application.Contracts.Messaging.Emails;
using Identity.Domain;
using Models.Responses;

namespace Identity.Application.User.RequestEmailChange;

internal sealed class RequestEmailChangeCommandHandler : ICommandHandler<RequestEmailChangeCommand>
{
    private readonly IEmailService _emailService;
    private readonly IUserRepository _userRepository;
    private readonly IUserContext _userContext;

    public RequestEmailChangeCommandHandler(
        IUserRepository userRepository,
        IEmailService emailService,
        IUserContext userContext)
    {
        _userRepository = userRepository;
        _emailService = emailService;
        _userContext = userContext;
    }

    public async Task<Result> Handle(RequestEmailChangeCommand request, CancellationToken cancellationToken)
    {
        var userIdGetResult = _userContext.GetUserId();

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

        var emailWriter = new EmailWriter(user.Email.Value, user.Username.Value);

        await _emailService.SendEmailAsync(
            emailWriter.WriteChangeEmail(verificationCode.Code),
            cancellationToken);

        var userUpdateResult = await _userRepository.UpdateAsync(user, cancellationToken);

        if (userUpdateResult.IsFailure)
        {
            return userUpdateResult.Error;
        }

        return Result.Success();
    }
}