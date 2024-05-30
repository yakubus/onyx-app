// Default URL for triggering event grid function in the local environment.
// http://localhost:7071/runtime/webhooks/EventGrid?functionName={functionname}
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Microsoft.Extensions.Logging;
using Azure.Messaging.EventGrid;
using Messanger.Functions.Models;
using Messanger.Functions.Services.Emails;
#pragma warning disable CS8509

namespace Messanger.Functions.Functions.Emails;

public sealed class EmailEventHandler
{
    private readonly EmailService _emailService;

    public EmailEventHandler(EmailService emailService)
    {
        _emailService = emailService;
    }

    [FunctionName(nameof(SendEmail))]
    public async Task SendEmail([EventGridTrigger] EventGridEvent eventGridEvent, ILogger log)
    {
        var emailData = eventGridEvent.Data.ToObjectFromJson<EmailData>();

        var result = await _emailService.SendAsync(emailData);

        if (result.IsFailure)
        {
            log.LogError("Email send operation failed for email: {emailRecipient}", emailData.Recipient);
            return;
        }

        log.LogInformation("Email sent to {emailRecipient}", emailData.Recipient);
    }
}