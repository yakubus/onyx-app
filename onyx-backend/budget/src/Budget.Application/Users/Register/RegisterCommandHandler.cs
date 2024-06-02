using Abstractions.Messaging;
using Budget.Application.Abstractions.Identity;
using Budget.Domain.Budgets;
using Budget.Domain.Users;
using Models.DataTypes;
using Models.Responses;

namespace Budget.Application.Users.Register;

internal sealed class RegisterCommandHandler : ICommandHandler<RegisterCommand, string>
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtService _jwtService;

    public RegisterCommandHandler(IUserRepository userRepository, IJwtService jwtService)
    {
        _userRepository = userRepository;
        _jwtService = jwtService;
    }

    public async Task<Result<string>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var isEmailAvailable = await _userRepository.GetByEmailAsync(request.Email, cancellationToken) 
            is { IsFailure: true };

        if (!isEmailAvailable)
        {
            return new Error("User.Email.AlreadyExists", "Email already in use");
        }

        var currencyCreateResult = Currency.FromCode(request.Currency);

        if (currencyCreateResult.IsFailure)
        {
            return currencyCreateResult.Error;
        }

        var user = new User(
            request.Email,
            request.Password,
            request.Username,
            currencyCreateResult.Value,
            Array.Empty<BudgetId>());

        var userCreateResult = await _userRepository.AddAsync(user, cancellationToken);

        if (userCreateResult.IsFailure)
        {
            return userCreateResult.Error;
        }

        user = userCreateResult.Value;

        var tokenCreateResult = _jwtService.GenerateJwt(user);

        if (tokenCreateResult.IsFailure)
        {
            return tokenCreateResult.Error;
        }

        return tokenCreateResult.Value;
    }
}