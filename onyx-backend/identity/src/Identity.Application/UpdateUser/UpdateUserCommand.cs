using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abstractions.Messaging;
using Identity.Application.Models;

namespace Identity.Application.UpdateUser;

public sealed record UpdateUserCommand(
    Guid UserId,
    string? NewEmail,
    string? NewUsername,
    string? NewPassword,
    string? NewCurrency) : ICommand<UserModel>
{
}