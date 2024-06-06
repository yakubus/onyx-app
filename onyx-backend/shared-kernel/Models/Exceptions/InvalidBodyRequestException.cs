namespace Models.Exceptions;

public sealed class InvalidBodyRequestException : Exception
{
    public InvalidBodyRequestException(
        Type expectedType) : base(
        $"Provided request contains invalid body type, expected body type: {expectedType.Name}")
    { }
}