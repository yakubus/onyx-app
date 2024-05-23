namespace Identity.Application.Messaging.Emails;

internal static class EmailTemplates
{
    internal static (string subject, string body) VerificationCodeBodyTemplate(string code) => (
        "", 
        """

        """);
}