using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Responses;

namespace Identity.Application.Errors;

internal static class BusinessErrors
{
    internal static readonly Error InvalidUserQueryRequest = new(
        "Application.InvalidUserQueryRequest",
        "Neither username, email nor id was provided");
    internal static readonly Error UserIsLoggedOut = new (
        "Application.UserIsLoggedOut",
        "User is logged out");
}