using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace Models.Responses;

public class Result
{
    protected internal Result(bool isSuccess, Error error)
    {
        switch (isSuccess)
        {
            case true when error != Error.None:
                throw new InvalidOperationException("Success result mustn't containe error data");
            case false when error == Error.None:
                throw new InvalidOperationException("Failed result must contain error data");
            default:
                IsSuccess = isSuccess;
                Error = error;
                break;
        }
    }

    protected Result()
    { }

    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

    public Error Error { get; }

    public static Result Success() => new(true, Error.None);

    public static Result Failure(Error error) => new(false, error);

    public static Result<TValue> Success<TValue>(TValue value) => new(value, true, Error.None);

    public static Result<TValue> Failure<TValue>(Error error) => new(default, false, error);

    public static Result<TValue> Create<TValue>(TValue? value) =>
        value is not null ? Success(value) : Failure<TValue>(Error.NullValue);

    public static Result<TValue?> CreateNullable<TValue>(TValue? value) => Success(value);
}

public class Result<TValue> : Result
{
    private readonly TValue? _value;

    [JsonConstructor]
    [System.Text.Json.Serialization.JsonConstructor]
    protected internal Result(TValue? value, bool isSuccess, Error error)
        : base(isSuccess, error)
    {
        _value = value;
    }

    [NotNullIfNotNull("_value")]
    public TValue Value => IsSuccess
        ? _value! : default;

    public static implicit operator Result<TValue>(TValue? value) => Create(value);

}