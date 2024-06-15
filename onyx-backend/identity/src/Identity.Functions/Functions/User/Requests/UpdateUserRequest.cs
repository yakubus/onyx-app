namespace Identity.Functions.Controllers.User.Requests;

public sealed record UpdateUserRequest(
    string? NewEmail,
    string? NewUsername,
    string? NewPassword,
    string? NewCurrency,
    string? VerificationCode);