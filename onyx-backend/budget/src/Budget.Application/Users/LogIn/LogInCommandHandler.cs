using Abstractions.Messaging;
using Budget.Application.Abstractions.Identity;
using Budget.Domain.Users;
using Models.Responses;

namespace Budget.Application.Users.LogIn;

internal sealed class LogInCommandHandler : ICommandHandler<LogInCommand, string>
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtService _jwtService;

    public LogInCommandHandler(IUserRepository userRepository, IJwtService jwtService)
    {
        _userRepository = userRepository;
        _jwtService = jwtService;
    }

    public async Task<Result<string>> Handle(LogInCommand request, CancellationToken cancellationToken)
    {
        var userGetResult = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);

        if (userGetResult.IsFailure)
        {
            return userGetResult.Error;
        }

        var user = userGetResult.Value;

        if (user.Password != request.Password)
        {
            return new Error("User.Unathorized", "Invalid credentials");
        }

        var tokenCreateResult = _jwtService.GenerateJwt(user);

        if (tokenCreateResult.IsFailure)
        {
            return tokenCreateResult.Error;
        }

        return tokenCreateResult.Value;
    }
}