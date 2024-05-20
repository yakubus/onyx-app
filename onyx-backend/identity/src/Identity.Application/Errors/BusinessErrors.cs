using Models.Responses;

namespace Identity.Application.Errors;

internal static class BusinessErrors
{
    internal static readonly Error InvalidUserQueryRequest = new(
        "Application.InvalidUserQueryRequest",
        "Neither username nor email was provided");
    internal static readonly Error UserIsLoggedOut = new (
        "Application.UserIsLoggedOut",
        "User is logged out");
}