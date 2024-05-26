using Identity.Domain;
using Identity.Infrastructure.Email.Models;
using Identity.Infrastructure.Messanger;
using Models.Responses;

namespace Identity.Infrastructure.Email;

internal sealed class EmailService : IEmailService
{
    private readonly MessangerClient _messangerClient;

    public EmailService(MessangerClient messangerClient)
    {
        _messangerClient = messangerClient;
    }

    public async Task<Result> SendEmailAsync(string recipient, string subject, string htmlBody, string plainTextBody)
    {
        var emailMessage = EmailMessage.Write(recipient, subject, htmlBody, plainTextBody);

        await _messangerClient.Message("SendEmail", emailMessage);

        return Result.Success();
    }

    public async Task<Result> SendEmailAsync((string recipient, string subject, string htmlBody, string plainTextBody) request, CancellationToken cancellationToken) =>
        await SendEmailAsync(request.recipient, request.subject, request.htmlBody, request.plainTextBody);
}