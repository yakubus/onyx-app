using Abstractions.Messaging;
using Identity.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Application.GetUser;

public sealed record GetUserQuery(Guid? UserId, string? Username, string? Email)
    : IQuery<UserModel>
{
}