using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;

namespace Identity.Infrastructure.Messanger;

internal sealed class MessangerClient
{
    private static readonly AmazonSimpleNotificationServiceClient snsClient = new ();

    public async Task Message(string topicArn, Dictionary<string, MessageAttributeValue> data)
    {
        var publishRequest = new PublishRequest
        {
            TopicArn = topicArn,
            MessageAttributes = data
        };

        _ = await snsClient.PublishAsync(publishRequest);
    }
}