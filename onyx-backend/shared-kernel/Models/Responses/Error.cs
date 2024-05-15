namespace Models.Responses;

public record Error(string Code, string Message)
{
    public static Error None = new(string.Empty, string.Empty);

    public static Error NullValue = new("Error.NullValue", "Null input");
    public static Error InvalidValue = new("Error.InvalidValue", "Invalid input");
    public static Error Exception = new("Error.Exception", "Internal server error");
    public static Error ValidationError = new("Error.Validation", "Validation error");
    public override string ToString() => $"{Code}: {Message}";
}