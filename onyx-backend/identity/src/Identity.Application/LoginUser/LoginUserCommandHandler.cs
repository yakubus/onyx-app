using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abstractions.Messaging;
using Identity.Application.Abstractions.Authentication;
using Identity.Application.Models;
using Identity.Domain;
using Models.Responses;

namespace Identity.Application.LoginUser;

internal sealed class LoginUserCommandHandler : ICommandHandler<LoginUserCommand, UserModel>
{
    private readonly IJwtService _jwtService;
    private readonly IUserRepository _userRepository;

    public LoginUserCommandHandler(IJwtService jwtService, IUserRepository userRepository)
    {
        _jwtService = jwtService;
        _userRepository = userRepository;
    }

    public async Task<Result<UserModel>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {


        return null;
    }
}