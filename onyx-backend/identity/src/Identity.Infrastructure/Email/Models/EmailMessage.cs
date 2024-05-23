using Models.Responses;

namespace Identity.Infrastructure.Email.Models;

internal sealed record EmailMessage
{
    public Domain.Email Recipient { get; init; }
    public EmailMessageSubject Subject { get; init; }
    public EmailMessageBody Body { get; init; }

    private EmailMessage(Domain.Email recipient, EmailMessageSubject subject, EmailMessageBody body)
    {
        Recipient = recipient;
        Subject = subject;
        Body = body;
    }

    public static Result<EmailMessage> Write(string recipient, string subject, string body)
    {
        var recipientEmailCreateResult = Domain.Email.Create(recipient);

        if (recipientEmailCreateResult.IsFailure)
        {
            return recipientEmailCreateResult.Error;
        }

        var messageSubjectCreateResult = EmailMessageSubject.Create(subject);

        if (messageSubjectCreateResult.IsFailure)
        {
            return messageSubjectCreateResult.Error;
        }

        var messageBodyCreateResult = EmailMessageBody.Create(body);

        if (messageBodyCreateResult.IsFailure)
        {
            return messageBodyCreateResult.Error;
        }

        return new EmailMessage(
            recipientEmailCreateResult.Value,
            messageSubjectCreateResult.Value,
            messageBodyCreateResult.Value);
    }
}