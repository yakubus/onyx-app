namespace Identity.API.Controllers.Auth.Requests;

public sealed record LoginRequest(string Email, string Password);