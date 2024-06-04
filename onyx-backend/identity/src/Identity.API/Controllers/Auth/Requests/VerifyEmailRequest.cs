namespace Identity.API.Controllers.Auth.Requests;

public sealed record VerifyEmailRequest(string Email, string VerificationCode);