using Abstractions.Messaging;
using Identity.Application.Abstractions.Authentication;
using Identity.Application.Errors;
using Identity.Application.Models;
using Identity.Application.Shared;
using Identity.Domain;
using Models.Responses;

namespace Identity.Application.GetUser;

internal sealed class GetUserQueryHandler : IQueryHandler<GetUserQuery, UserModel>
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtService _jwtService;

    public GetUserQueryHandler(IUserRepository userRepository, IJwtService jwtService)
    {
        _userRepository = userRepository;
        _jwtService = jwtService;
    }

    public async Task<Result<UserModel>> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        if (request.UserId is null && request.Email is null)
        {
            return Result.Failure<UserModel>(BusinessErrors.InvalidUserQueryRequest);
        }

        var userQueryService = new UserQueryService(_userRepository);

        var userGetResult = await userQueryService.GetUser(
            request.UserId,
            request.Email,
            cancellationToken);

        if (userGetResult.IsFailure)
        {
            return Result.Failure<UserModel>(userGetResult.Error);
        }

        var user = userGetResult.Value;

        if (!user.IsAuthenticated)
        {
            return UserModel.FromDomainModel(user);
        }

        var tokenGenerateResult = _jwtService.GenerateJwt(user);

        if (tokenGenerateResult.IsFailure)
        {
            return tokenGenerateResult.Error;
        }

        var token = tokenGenerateResult.Value;

        return UserModel.FromDomainModel(user, new (token));
    }
}