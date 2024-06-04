namespace Identity.API.Controllers.Auth.Requests;

public sealed record RefreshRequest(string LongLivedToken);