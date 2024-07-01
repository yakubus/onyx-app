namespace Models.Responses;

public record Error(string Code, string Message)
{
    public static Error None = new(string.Empty, string.Empty);

    public static Error NullValue = new("Error.NullValue", "Null input");
    public static Error InvalidValue = new("Error.InvalidValue", "Invalid input");
    public static Error Exception = new("Error.Exception", "Internal server error");
    public static Error ValidationError(IEnumerable<string?> members) => new("Error.Validation", $"Validation failed for: {string.Join(',', members)}");

    /// <summary>
    /// Use only for logging, do not return this error to user
    /// </summary>
    /// <param name="e"></param>
    /// <returns></returns>
    public static Error ExceptionWithMessage(Exception e) => new("Error.Exception", $"{e.Message}");
    public static Error ExceptionWithMessage(string eMessage) => new("Error.Exception", $"{eMessage}");
    public override string ToString() => $"{Code}: {Message}";
}