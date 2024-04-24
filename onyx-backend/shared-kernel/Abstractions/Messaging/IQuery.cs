namespace Abstractions.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}