using Models.Responses;

namespace Identity.Application.Contracts.Errors;

internal static class BusinessErrors
{
    internal static readonly Error InvalidUserQueryRequest = new(
        "Application.InvalidUserQueryRequest",
        "Email was not provided");
    internal static readonly Error UserIsLoggedOut = new(
        "Application.UserIsLoggedOut",
        "User is logged out");
    internal static readonly Error EmailAlreadyInUse = new(
        "Application.EmailAlreadyInUse",
        "Email is already in use");
}