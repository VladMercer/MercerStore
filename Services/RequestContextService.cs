using MercerStore.Interfaces;

namespace MercerStore.Services
{
    public class RequestContextService : IRequestContextService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RequestContextService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void SetLogDetails(object details)
        {
            _httpContextAccessor.HttpContext.Items["LogDetails"] = details;
        }

        public object? GetLogDetails()
        {
            return _httpContextAccessor.HttpContext.Items["LogDetails"];
        }
    }
}
