using Abstractions.Messaging;
using Identity.Application.Abstractions.Authentication;
using Identity.Application.Contracts.Errors;
using Identity.Application.Contracts.Models;
using Identity.Application.Contracts.Shared;
using Identity.Domain;
using Models.Responses;

namespace Identity.Application.User.GetUser;

internal sealed class GetUserQueryHandler : IQueryHandler<GetUserQuery, UserModel>
{
    private readonly IUserRepository _userRepository;
    private readonly IUserContext _userContext;

    public GetUserQueryHandler(IUserRepository userRepository, IUserContext userContext)
    {
        _userRepository = userRepository;
        _userContext = userContext;
    }

    public async Task<Result<UserModel>> Handle(GetUserQuery request, CancellationToken cancellationToken)
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
            return Result.Failure<UserModel>(userGetResult.Error);
        }

        var user = userGetResult.Value;

        return UserModel.FromDomainModel(user);
    }
}