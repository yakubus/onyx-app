namespace Identity.Functions.Functions.Auth.Requests;

public sealed record ResendEmailRequest(string Email, string MessageType);