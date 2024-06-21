namespace Budget.Functions.Logger;

internal static class LoggerOptions
{
    public static readonly List<string> Using = ["AWS.Logger.SeriLog"];
    public const string LogGroup = "Serliog.Onyx";
    public const string Region = "eu-central-1";
    public const string MinimumLevel = "Information";
}