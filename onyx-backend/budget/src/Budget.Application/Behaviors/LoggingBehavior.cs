using MediatR;
using Microsoft.Extensions.Logging;
using Models.Responses;

namespace Budget.Application.Behaviors;

public sealed class LoggingBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IBaseRequest
    where TResponse : Result
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        //using (LogContext.PushProperty("RequestHandler", typeof(TRequest).FullName))
        //{
            LogHandleStart();

            var response = await next();

            if (response.IsSuccess)
            {
                LogHandleSuccess();
                return response;
            }

            LogHandleFailure(response);

            return response;
        //}
    }

    private void LogHandleFailure(TResponse response)
    {
        _logger.LogWarning("Failed to handle {request}: {error}", typeof(TRequest).Name, response.Error);
    }

    private void LogHandleSuccess()
    {
        _logger.LogInformation($"Successfully handled {typeof(TRequest).Name}");
    }

    private void LogHandleStart()
    {
        _logger.LogInformation($"Handling {typeof(TRequest).Name}");
    }
}