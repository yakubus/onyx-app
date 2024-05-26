using Abstractions.Messaging;
using Identity.Application.Abstractions.Authentication;
using Identity.Application.Errors;
using Identity.Application.Models;
using Identity.Domain;
using Models.Responses;

namespace Identity.Application.RegisterUser;

internal sealed class RegisterUserCommandHandler : ICommandHandler<RegisterUserCommand, UserModel>
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtService _jwtService;

    public RegisterUserCommandHandler(IUserRepository userRepository, IJwtService jwtService)
    {
        _userRepository = userRepository;
        _jwtService = jwtService;
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

        var userRegisterResult = User.Register(request.Email, request.Username, request.Password, request.Currency);

        if (userRegisterResult.IsFailure)
        {
            return userRegisterResult.Error;
        }

        var user = userRegisterResult.Value;

        var userAddResult = await _userRepository.AddAsync(user, cancellationToken);

        if (userAddResult.IsFailure)
        {
            return userAddResult.Error;
        }

        user = userAddResult.Value;

        var tokenGenerateResult = _jwtService.GenerateJwt(user);

        if (tokenGenerateResult.IsFailure)
        {
            return tokenGenerateResult.Error;
        }

        var token = tokenGenerateResult.Value;

        return UserModel.FromDomainModel(user, new (token));
    }
}