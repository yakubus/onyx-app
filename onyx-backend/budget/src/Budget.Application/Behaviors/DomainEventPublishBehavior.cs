using Abstractions.Messaging;
using MediatR;
using Microsoft.Extensions.Logging;
using Models.Responses;

namespace Budget.Application.Behaviors;

public class DomainEventPublishBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IBaseCommand
    where TResponse : Result
{
    private readonly IPublisher _publisher;
    private readonly ILogger<DomainEventPublishBehavior<TRequest, TResponse>> _logger;

    public DomainEventPublishBehavior(IPublisher publisher, ILogger<DomainEventPublishBehavior<TRequest, TResponse>> logger)
    {
        _publisher = publisher;
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var response = await next();

        var domainEvents = GetDomainEvents(response);

        if (domainEvents is null || domainEvents.Count < 1)
        {
            return response;
        }

        //using (LogContext.PushProperty("DomainEvents", domainEvents, true))
        //{
            LogDomainEventPublishingStart(domainEvents);

            foreach (var domainEvent in domainEvents)
            {
                LogSingleDomainEventPublishingStart(domainEvent);

                await _publisher.Publish(domainEvent, cancellationToken);

                LogSingleDomainEventPublishingSuccess(domainEvent);
            }

            LogDomainEventPublishingSuccess(domainEvents);

            return response;
        //}
    }

    private void LogDomainEventPublishingSuccess(IReadOnlyList<IDomainEvent> domainEvents)
    {
        _logger.LogInformation(
            $"The process of publishing domain events is completed, {domainEvents.Count} domain events were published");
    }

    private void LogSingleDomainEventPublishingSuccess(IDomainEvent domainEvent)
    {
        _logger.LogInformation("Domain event {DomainEvent} published", domainEvent.GetType().FullName);
    }

    private void LogSingleDomainEventPublishingStart(IDomainEvent domainEvent)
    {
        _logger.LogInformation("Publishing domain event {DomainEvent}", domainEvent.GetType().FullName);
    }

    private void LogDomainEventPublishingStart(IReadOnlyList<IDomainEvent> domainEvents)
    {
        _logger.LogInformation(
            $"The process of publishing domain events has started, {domainEvents.Count} domain events are up to be published");
    }

    private static List<IDomainEvent>? GetDomainEvents(Result result)
    {
        if (result.IsFailure)
        {
            return null;
        }

        var value = GetProperty(result, "Value");

        if (value is null)
        {
            return null;
        }

        if (!value.GetType().IsArray)
        {
            return GetDomainEvents(value);
        }

        List<IDomainEvent> domainEvents = new();
                
        foreach (var val in value as Array ?? new object[] { })
        {
            var entityDomainEvents = GetDomainEvents(val);
                    
            if (entityDomainEvents is null)
            {
                continue;
            }

            domainEvents.AddRange(entityDomainEvents);
        }

        return domainEvents;

    }

    private static List<IDomainEvent>? GetDomainEvents(object value) =>
        value.GetType().GetMethod("GetDomainEvents")?.Invoke(value, null) as List<IDomainEvent>;

    private static object? GetProperty(object obj, string propName) => obj.GetType().GetProperty(propName)?.GetValue(obj);
}