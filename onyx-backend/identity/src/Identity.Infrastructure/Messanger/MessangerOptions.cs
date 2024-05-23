namespace Identity.Infrastructure.Messanger;

public sealed class MessangerOptions
{
    public string TopicEndpoint { get; init; } = string.Empty;
    public string TopicKey { get; init; } = string.Empty;
}