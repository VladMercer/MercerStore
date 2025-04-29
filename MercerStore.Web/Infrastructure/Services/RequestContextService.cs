using MercerStore.Web.Application.Interfaces.Services;

namespace MercerStore.Web.Infrastructure.Services;

public class RequestContextService : IRequestContextService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public RequestContextService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public void SetLogDetails(object details)
    {
        if (_httpContextAccessor.HttpContext != null) _httpContextAccessor.HttpContext.Items["LogDetails"] = details;
    }

    public object? GetLogDetails()
    {
        return _httpContextAccessor.HttpContext?.Items["LogDetails"];
    }
}
