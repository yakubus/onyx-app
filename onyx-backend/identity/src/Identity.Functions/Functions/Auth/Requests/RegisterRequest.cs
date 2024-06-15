namespace Identity.Functions.Controllers.Auth.Requests;

public sealed record RegisterRequest(string Email, string Username, string Password, string Currency);