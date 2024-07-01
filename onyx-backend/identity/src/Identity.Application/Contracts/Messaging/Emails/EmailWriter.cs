namespace Identity.Application.Contracts.Messaging.Emails;

internal sealed class EmailWriter
{
    private readonly string _recipientEmail;
    private readonly string _username;

    public EmailWriter(string recipientEmail, string username)
    {
        _recipientEmail = recipientEmail;
        _username = username;
    }

    internal (string recipient, string subject, string htmlBody, string plainTextBody) WriteEmailVerification(string code)
    {
        var (subject, htmlBody, plainTextBody) = EmailTemplates.EmailVerificationBodyTemplate(code, _username);

        return (_recipientEmail, subject, htmlBody, plainTextBody);
    }

    internal (string recipient, string subject, string htmlBody, string plainTextBody) WriteForgotPassword(string code)
    {
        var (subject, htmlBody, plainTextBody) = EmailTemplates.ForgotPasswordBodyTemplate(code);

        return (_recipientEmail, subject, htmlBody, plainTextBody);
    }

    internal (string recipient, string subject, string htmlBody, string plainTextBody) WriteChangeEmail(string code)
    {
        var (subject, htmlBody, plainTextBody) = EmailTemplates.EmailChangeBodyTemplate(code, _username);

        return (_recipientEmail, subject, htmlBody, plainTextBody);
    }
}