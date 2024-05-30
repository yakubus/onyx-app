namespace Budget.API.Controllers.Users.Requests;

public sealed record RegisterRequest(string Email, string Password, string Currency, string Username)
{
}