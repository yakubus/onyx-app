using Abstractions.Messaging;
using Identity.Application.Abstractions.Authentication;
using Models.Responses;

namespace Identity.Application.RequestEmailChange;

internal sealed class RequestEmailChangeCommandHandler : ICommandHandler<RequestEmailChangeCommand>
{
    private readonly IJwtService _jwtService;

    public RequestEmailChangeCommandHandler(IJwtService jwtService)
    {
        _jwtService = jwtService;
    }

    public async Task<Result> Handle(RequestEmailChangeCommand request, CancellationToken cancellationToken)
    {
        return null;
    }
}