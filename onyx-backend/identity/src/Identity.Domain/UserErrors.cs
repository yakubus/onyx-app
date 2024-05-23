using Models.Responses;

namespace Identity.Domain;

internal static class UserErrors
{
    internal static readonly Error PasswordTooWeak = new ("User.Password.TooWeak", "Password is too weak");
    internal static readonly Error InvalidUsername = new ("User.Username.Invalid", "Invalid username");
    internal static readonly Error InvalidCredentials = new ("User.InvalidCredentials", "Invalid credentials");
    internal static readonly Error InvalidEmail = new ("User.Email.Invalid", "Invalid email");
    internal static readonly Error EmailChangeNotRequested = new (
        "User.Email.ChangeNotRequested",
        "Email change not requested");
    internal static readonly Error EmailNotVerified = new (
        "User.Email.NotVerified",
        "Email is not verified");
    public static readonly Error PasswordChangeNotRequested = new (
        "User.Password.ChangeNotRequested",
        "Password change not requested");
}