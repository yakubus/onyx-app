using Models.Responses;

namespace Identity.Application.Auth.ResendEmail;

internal sealed record MessageType
{
    public string Value { get; init; }
    private static readonly Error _invalidValueError = new Error(
        "MessageType.InvalidValue",
        "Invalid message type");

    private MessageType(string value) => Value = value;

    public static readonly MessageType EmailChangeRequest = new(nameof(EmailChangeRequest));
    public static readonly MessageType EmailVerification = new(nameof(EmailVerification));
    public static readonly MessageType ForgotPassword = new(nameof(ForgotPassword));

    public static Result<MessageType> FromString(string value) =>
        All.FirstOrDefault(mt => mt.Value.ToLower() == value.ToLower()) ??
        Result.Failure<MessageType>(_invalidValueError);

    public static IReadOnlyCollection<MessageType> All => [EmailChangeRequest, EmailVerification, ForgotPassword];
}