using MercerStore.Web.Application.Interfaces;
using MercerStore.Web.Application.Models;
using Serilog.Context;
namespace MercerStore.Web.Infrastructure.Services
{
    public class LogService : ILogService
    {
        private readonly ILogger<LogService> _logger;

        public LogService(ILogger<LogService> logger)
        {
            _logger = logger;
        }

        public void LogUserAction<TEntityId>(IEnumerable<string>? roles, string? userId, string action, string entityName, TEntityId? entityId, object? details)
        {
            var userActionLog = new UserActionLog<TEntityId>
            {
                Roles = roles,
                UserId = userId,
                Action = action,
                EntityName = entityName,
                EntityId = entityId,
                Details = details ?? new { },
            };

            using var disposable = LogContext.PushProperty("CustomLog", true);
            _logger.LogInformation("{@userActionLog}", userActionLog);

        }

        public void LogError(string message, Exception exception)
        {
            _logger.LogError(exception, message);
        }

        public void LogInformation(string message, object details)
        {
            _logger.LogInformation(message, details);
        }
    }
}
