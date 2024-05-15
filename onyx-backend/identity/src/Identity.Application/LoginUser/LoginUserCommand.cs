using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abstractions.Messaging;
using Identity.Application.Models;

namespace Identity.Application.LoginUser;

public sealed record LoginUserCommand(string? Username, string? Email, string Password)
    : ICommand<UserModel>
{
}