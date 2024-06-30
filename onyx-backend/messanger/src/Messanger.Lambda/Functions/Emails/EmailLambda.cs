using Amazon.Lambda.Core;
using Amazon.Lambda.SNSEvents;
using Messanger.Lambda.Models;
using Messanger.Lambda.Services.Emails;
using Newtonsoft.Json;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace Messanger.Lambda.Functions.Emails;

public sealed class EmailLambda
{
    private readonly JsonSerializationException _invalidEventException =
        new($"The provided event message was invalid for type {nameof(EmailData)}");

    public EmailLambda()
    { }


    public async Task FunctionHandler(SNSEvent snsEvent, ILambdaContext context)
    {
        foreach (var record in snsEvent.Records)
        {
            var messageAttributes = record.Sns.MessageAttributes;
            var data = EmailData.FromMessageAttributes(messageAttributes);

            await EmailService.SendAsync(data);
        }
    }
}