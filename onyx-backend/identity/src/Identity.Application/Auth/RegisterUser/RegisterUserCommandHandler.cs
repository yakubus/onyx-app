using Abstractions.Messaging;
using Identity.Application.Abstractions.Authentication;
using Identity.Application.Contracts.Errors;
using Identity.Application.Contracts.Messaging.Emails;
using Identity.Application.Contracts.Models;
using Identity.Domain;
using Models.Responses;

namespace Identity.Application.Auth.RegisterUser;

internal sealed class RegisterUserCommandHandler : ICommandHandler<RegisterUserCommand, UserModel>
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtService _jwtService;
    private IEmailService _emailService;

    public RegisterUserCommandHandler(IUserRepository userRepository, IJwtService jwtService, IEmailService emailService)
    {
        _userRepository = userRepository;
        _jwtService = jwtService;
        _emailService = emailService;
    }

    public async Task<Result<UserModel>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var emailCreateResult = Email.Create(request.Email);

        if (emailCreateResult.IsFailure)
        {
            return emailCreateResult.Error;
        }

        var isEmailUnique = await _userRepository.GetByEmailAsync(emailCreateResult.Value, cancellationToken) is
        {
            IsFailure: true
        };

        if (!isEmailUnique)
        {
            return BusinessErrors.EmailAlreadyInUse;
        }

        var userRegisterResult = Domain.User.Register(
            request.Email,
            request.Username,
            request.Password,
            request.Currency);

        if (userRegisterResult.IsFailure)
        {
            return userRegisterResult.Error;
        }

        var user = userRegisterResult.Value;

        var emailWriter = new EmailWriter(user.Email.Value, user.Username.Value);

        await _emailService.SendEmailAsync(
            emailWriter.WriteEmailVerification(user.VerificationCode!.Code),
            cancellationToken);

        var userAddResult = await _userRepository.AddAsync(user, cancellationToken);

        if (userAddResult.IsFailure)
        {
            return userAddResult.Error;
        }

        user = userAddResult.Value;

        return UserModel.FromDomainModel(user);
    }
}