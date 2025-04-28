using MediatR;
using MercerStore.Web.Application.Interfaces.Services;
using MercerStore.Web.Application.Requests.Log;

namespace MercerStore.Web.Application.Handlers;

public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : LoggableRequest<TResponse>
{
    private readonly ILogService _logService;
    private readonly IUserIdentifierService _userIdentifierService;

    public LoggingBehavior(ILogService logService, IUserIdentifierService userIdentifierService)
    {
        _logService = logService;
        _userIdentifierService = userIdentifierService;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken ct)
    {
        var response = await next();

        var roles = _userIdentifierService.GetCurrentUserRoles();
        var userId = _userIdentifierService.GetCurrentIdentifier();

        _logService.LogUserAction(
            roles,
            userId,
            request.Action,
            request.EntityName,
            request.EntityId,
            request.Details
        );

        return response;
    }
}