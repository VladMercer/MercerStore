using MercerStore.Interfaces;
using MercerStore.Models;
using Serilog.Context;
namespace MercerStore.Services
{
    public class LogService : ILogService
    {
        private readonly ILogger<LogService> _logger;

        public LogService(ILogger<LogService> logger)
        {
            _logger = logger;
        }



        public void LogUserAction(IEnumerable<string>? roles, string? userId, string action, string entityName, int? entityId, object? details)
        {
            var userActionLog = new UserActionLog
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
