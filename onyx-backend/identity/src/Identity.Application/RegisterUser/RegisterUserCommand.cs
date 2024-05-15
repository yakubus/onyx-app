using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abstractions.Messaging;
using Identity.Application.Models;

namespace Identity.Application.RegisterUser;

public sealed record RegisterUserCommand(string Email, string Password, string Currency) 
    : ICommand<UserModel>
{
}