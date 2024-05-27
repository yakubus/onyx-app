using Abstractions.Messaging;
using Budget.Application.Abstractions.Identity;
using Budget.Application.Users.Models;
using Budget.Domain.Users;
using Models.Responses;

namespace Budget.Application.Users.GetUser;

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
        var getUserIdResult = _userContext.GetUserId();

        if (getUserIdResult.IsFailure)
        {
            return getUserIdResult.Error;
        }

        var userGetResult = await _userRepository.GetByIdAsync(new (getUserIdResult.Value), cancellationToken);

        if (userGetResult.IsFailure)
        {
            return userGetResult.Error;
        }

        var user = userGetResult.Value;

        return UserModel.FromDomainModel(user);
    }
}