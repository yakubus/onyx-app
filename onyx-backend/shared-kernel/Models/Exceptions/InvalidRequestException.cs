namespace Models.Exceptions;

public sealed class InvalidRequestException : Exception
{
    public InvalidRequestException(
        Type expectedType) : base(
        $"Invalid request type, expected type of request: {expectedType.Name}")
    { }
}