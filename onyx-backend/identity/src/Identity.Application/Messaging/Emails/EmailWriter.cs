namespace Identity.Application.Messaging.Emails;

internal sealed class EmailWriter
{
    private readonly string _recipientEmail;

    public EmailWriter(string recipientEmail)
    {
        _recipientEmail = recipientEmail;
    }

    internal (string recipient, string subject, string htmlBody, string plainTextBody) WriteVerificationCode(string code)
    {
        var (subject, htmlBody, plainTextBody) = EmailTemplates.VerificationCodeBodyTemplate(code);

        return (_recipientEmail, subject, htmlBody, plainTextBody);
    }
}