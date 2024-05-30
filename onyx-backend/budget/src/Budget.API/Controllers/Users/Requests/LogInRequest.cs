namespace Budget.API.Controllers.Users.Requests;

public sealed record LogInRequest(string Email, string Password)
{
}