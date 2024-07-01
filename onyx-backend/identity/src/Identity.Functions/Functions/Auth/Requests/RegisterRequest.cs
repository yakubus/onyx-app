namespace Identity.Functions.Functions.Auth.Requests;

public sealed record RegisterRequest(string Email, string Username, string Password, string Currency);