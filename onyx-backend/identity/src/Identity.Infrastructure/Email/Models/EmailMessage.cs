using Models.Responses;

namespace Identity.Infrastructure.Email.Models;

internal sealed record EmailMessage
{
    public Domain.Email Recipient { get; init; }
    public EmailMessageSubject Subject { get; init; }
    public EmailMessageBody HtmlBody { get; init; }
    public EmailMessageBody PlainTextBody { get; init; }

    private EmailMessage(Domain.Email recipient, EmailMessageSubject subject, EmailMessageBody plainTextBody, EmailMessageBody htmlBody)
    {
        Recipient = recipient;
        Subject = subject;
        PlainTextBody = plainTextBody;
        HtmlBody = htmlBody;
    }

    public static Result<EmailMessage> Write(string recipient, string subject, string htmlBody, string plainTextBody)
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

        var htmlBodyCreateResult = EmailMessageBody.CreateHtml(htmlBody);

        if (htmlBodyCreateResult.IsFailure)
        {
            return htmlBodyCreateResult.Error;
        }

        var plainTextBodyCreateResult = EmailMessageBody.CreatePlainText(plainTextBody);

        return new EmailMessage(
            recipientEmailCreateResult.Value,
            messageSubjectCreateResult.Value,
            htmlBodyCreateResult.Value,
            plainTextBodyCreateResult.Value);
    }
}