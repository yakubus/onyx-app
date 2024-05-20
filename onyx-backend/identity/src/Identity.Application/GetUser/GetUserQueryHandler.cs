using Abstractions.Messaging;
using Identity.Application.Errors;
using Identity.Application.Models;
using Identity.Application.Shared;
using Identity.Domain;
using Models.Responses;

namespace Identity.Application.GetUser;

internal sealed class GetUserQueryHandler : IQueryHandler<GetUserQuery, UserModel>
{
    private readonly IUserRepository _userRepository;

    public GetUserQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result<UserModel>> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        if (request.UserId is null && request.Email is null && request.Username is null)
        {
            return Result.Failure<UserModel>(BusinessErrors.InvalidUserQueryRequest);
        }

        var userQueryService = new UserQueryService(_userRepository);

        var userGetResult = await userQueryService.GetUser(
            request.UserId,
            request.Email,
            request.Username,
            cancellationToken);

        if (userGetResult.IsFailure)
        {
            return Result.Failure<UserModel>(userGetResult.Error);
        }

        var user = userGetResult.Value;

        return UserModel.FromDomainModel(user);
    }
}