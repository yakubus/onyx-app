namespace Models.Responses;

public record Error(string Code, string Message)
{
    public static Error None = new(string.Empty, string.Empty);

    public static Error NullValue = new("Error.NullValue", "Passed null value");

    public override string ToString() => $"{Code}: {Message}";
}