namespace Models.Exceptions;

public sealed class DomainException<T>(string message) : DomainException(message, typeof(T));

public abstract class DomainException(string message, Type type) : Exception(message)
{
    public Type Type { get; init; } = type;
}