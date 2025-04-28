using MediatR;
using MercerStore.Web.Application.Requests.Log;

namespace MercerStore.Web.Application.Handlers.Account.Commands;

public record LogoutCommand() : LoggableRequest<Unit>("User logged out", "User");

public class LogoutHandler : IRequestHandler<LogoutCommand, Unit>
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public LogoutHandler(IHttpContextAccessor contextAccessor)
    {
        _httpContextAccessor = contextAccessor;
    }

    public async Task<Unit> Handle(LogoutCommand request, CancellationToken ct)
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext != null) httpContext.Response.Cookies.Delete("OohhCookies");

        return Unit.Value;
    }
}