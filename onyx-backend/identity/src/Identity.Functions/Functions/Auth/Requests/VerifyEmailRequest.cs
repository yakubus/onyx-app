namespace Identity.Functions.Functions.Auth.Requests;

public sealed record VerifyEmailRequest(string Email, string VerificationCode);