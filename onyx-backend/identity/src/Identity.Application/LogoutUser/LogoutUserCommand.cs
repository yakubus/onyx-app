using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abstractions.Messaging;
using Identity.Application.Models;

namespace Identity.Application.LogoutUser;

public sealed record LogoutUserCommand(Guid? UserId, string? Username, string? Email)
    : ICommand
{
}