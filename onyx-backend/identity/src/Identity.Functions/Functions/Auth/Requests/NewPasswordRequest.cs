namespace Identity.Functions.Functions.Auth.Requests;

public sealed record NewPasswordRequest(string Email, string NewPassword, string VerificationCode);