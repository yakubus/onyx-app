namespace Identity.API.Controllers.Auth.Requests;

public sealed record ResendEmailRequest(string Email, string MessageType);