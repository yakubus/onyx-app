namespace Identity.Functions.Functions.User.Requests;

public sealed record UpdateUserRequest(
    string? NewEmail,
    string? NewUsername,
    string? NewCurrency,
    string? VerificationCode);