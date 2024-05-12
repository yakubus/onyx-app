namespace Budget.Infrastructure.Data;

public sealed class CosmosDbOptions
{
    public string AccountUri { get; init; } = string.Empty;
    public string PrimaryKey { get; init; } = string.Empty;
    public string Database { get; init; } = string.Empty;
}