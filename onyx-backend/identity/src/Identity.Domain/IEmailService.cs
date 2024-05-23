using Models.Responses;

namespace Identity.Domain;

public interface IEmailService
{
    Task<Result> SendEmailAsync(string recipient, string subject, string message);

    Task<Result> SendEmailAsync((string recipient, string subject, string body) request, CancellationToken cancellationToken);
}