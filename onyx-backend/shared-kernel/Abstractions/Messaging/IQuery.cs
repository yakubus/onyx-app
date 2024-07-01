using MediatR;
using Models.Responses;

namespace Abstractions.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>;