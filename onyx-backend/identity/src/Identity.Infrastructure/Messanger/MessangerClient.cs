using Azure.Messaging.EventGrid;
using Azure;
using Microsoft.Extensions.Options;

namespace Identity.Infrastructure.Messanger;

internal sealed class MessangerClient
{
    private readonly EventGridPublisherClient _client;

    public MessangerClient(IOptions<MessangerOptions> messangerOptions)
    {
        _client = new EventGridPublisherClient(
            new Uri(messangerOptions.Value.TopicEndpoint),
            new AzureKeyCredential(messangerOptions.Value.TopicKey));
    }

    public async Task Message(string subject, object data)
    {
        var eventGridEvent = new EventGridEvent(
            subject: subject,
            eventType: $"Onyx.Events.{subject}",
            dataVersion: "1.0",
            data: data
        );

        await _client.SendEventAsync(eventGridEvent);
    }
}