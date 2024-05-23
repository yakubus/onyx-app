namespace Identity.Application.Messaging.Emails;

internal sealed class EmailWriter
{
    private readonly string _recipientEmail;

    public EmailWriter(string recipientEmail)
    {
        _recipientEmail = recipientEmail;
    }

    internal (string recipient, string subject, string body) WriteVerificationCode(string code)
    {
        var (subject, body) = EmailTemplates.VerificationCodeBodyTemplate(code);

        return (_recipientEmail, subject, body);
    }
}