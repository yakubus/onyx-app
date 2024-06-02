using Models.Responses;

namespace Identity.Infrastructure.Email.Models;

internal sealed record EmailMessageSubject
{
    public string Value { get; init; }

    private EmailMessageSubject(string value)
    {
        Value = value;
    }

    public static Result<EmailMessageSubject> Create(string subject)
    {
        return new EmailMessageSubject(subject);
    }
}