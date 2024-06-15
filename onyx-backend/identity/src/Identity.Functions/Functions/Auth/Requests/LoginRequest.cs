namespace Identity.Functions.Controllers.Auth.Requests;

public sealed record LoginRequest(string Email, string Password);