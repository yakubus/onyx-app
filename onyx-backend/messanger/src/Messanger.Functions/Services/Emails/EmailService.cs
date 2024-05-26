using System.Threading.Tasks;
using Azure;
using Messanger.Functions.Models;
using Models.Responses;
using Azure.Communication.Email;
using Messanger.Functions.Settings.Email;
using Microsoft.Extensions.Options;

namespace Messanger.Functions.Services.Emails;

public sealed class EmailService
{
    private readonly EmailOptions emailOptions;

    public EmailService(IOptions<EmailOptions> emailOptions)
    {
        this.emailOptions = emailOptions.Value;
    }

    //TODO push the logic of retry to this method
    public async Task<Result> SendAsync(EmailData data, CancellationToken cancellationToken = default)
    {
        var emailClient = new EmailClient(emailOptions.ConnectionString);

        try
        {
            var response = await emailClient.SendAsync(
                WaitUntil.Completed,
                senderAddress: "DoNotReply@<from_domain>",
                recipientAddress: "<to_email>",
                subject: "Test Email",
                htmlContent: "<html><h1>Hello world via email.</h1></html>",
                plainTextContent: "Hello world via email.",
                cancellationToken);


            return response.Value.Status == EmailSendStatus.Succeeded ?
                Result.Success() :
                Result.Failure(Error.None);
        }
        catch (RequestFailedException e)
        {
            return Result.Failure(Error.ExceptionWithMessage(e));
        }
    }
}