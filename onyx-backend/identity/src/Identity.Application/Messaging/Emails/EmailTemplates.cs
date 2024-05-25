namespace Identity.Application.Messaging.Emails;

internal static class EmailTemplates
{
    //TODO implement email template
    internal static (string subject, string htmlBody, string plainTextBody) VerificationCodeBodyTemplate(string code) => (
        "", 
        """

        """,
        """
        
        """);
}