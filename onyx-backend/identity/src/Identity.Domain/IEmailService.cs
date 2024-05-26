using Models.Responses;

namespace Identity.Domain;

public interface IEmailService
{
    Task<Result> SendEmailAsync(string recipient, string subject, string htmlBody, string plainTextBody);

    Task<Result> SendEmailAsync((string recipient, string subject, string htmlBody, string plainTextBody) request, CancellationToken cancellationToken);
}