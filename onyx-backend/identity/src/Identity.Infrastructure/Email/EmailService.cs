using Amazon.SimpleNotificationService.Model;
using Identity.Domain;
using Identity.Infrastructure.Email.Models;
using Identity.Infrastructure.Messanger;
using Models.Responses;

namespace Identity.Infrastructure.Email;

internal sealed class EmailService : IEmailService
{
    private readonly MessangerClient _messangerClient;
    private const string topicArn = "arn:aws:sns:eu-central-1:975049887576:onyx-messanger-SendEmail-IW5o0b2XbfUF";

    public EmailService(MessangerClient messangerClient)
    {
        _messangerClient = messangerClient;
    }

    public async Task<Result> SendEmailAsync(string recipient, string subject, string htmlBody, string plainTextBody)
    {
        var emailMessageWriteResult = EmailMessage.Write(recipient, subject, htmlBody, plainTextBody);

        if (emailMessageWriteResult.IsFailure)
        {
            return emailMessageWriteResult.Error;
        }

        var message = emailMessageWriteResult.Value;

        var data = new Dictionary<string, MessageAttributeValue>
        {
            { nameof(EmailMessage.Recipient), new MessageAttributeValue {DataType = "String", StringValue = message.Recipient.Value} },
            { nameof(EmailMessage.Subject), new MessageAttributeValue {DataType = "String", StringValue = message.Subject.Value} },
            { nameof(EmailMessage.HtmlBody), new MessageAttributeValue {DataType = "String", StringValue = message.HtmlBody.Value} },
            { nameof(EmailMessage.PlainTextBody), new MessageAttributeValue {DataType = "String", StringValue = message.PlainTextBody.Value} },
        };

        await _messangerClient.Message(topicArn, data);

        return Result.Success();
    }

    public async Task<Result> SendEmailAsync((string recipient, string subject, string htmlBody, string plainTextBody) request, CancellationToken cancellationToken) =>
        await SendEmailAsync(request.recipient, request.subject, request.htmlBody, request.plainTextBody);
}