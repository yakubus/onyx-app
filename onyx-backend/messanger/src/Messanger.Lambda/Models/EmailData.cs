using Amazon.Lambda.SNSEvents;

namespace Messanger.Lambda.Models;

public sealed record EmailData
{
    public string Recipient { get; init; }
    public string Subject { get; init; }
    public string HtmlBody { get; init; }
    public string PlainTextBody { get; init; }

    private EmailData(string recipient, string subject, string htmlBody, string plainTextBody)
    {
        Recipient = recipient;
        Subject = subject;
        HtmlBody = htmlBody;
        PlainTextBody = plainTextBody;
    }

    public static EmailData FromMessageAttributes(IDictionary<string, SNSEvent.MessageAttribute> messageAttributes)
    {
        return new(
            messageAttributes[nameof(Recipient)].Value,
            messageAttributes[nameof(Subject)].Value,
            messageAttributes[nameof(HtmlBody)].Value,
            messageAttributes[nameof(PlainTextBody)].Value);
    }
}