namespace MercerStore.Interfaces
{
    public interface ILogService
    {
        void LogUserAction(IEnumerable<string?> roles, string? userId, string action, string entityName, int? entityId, object? details);
        void LogError(string message, Exception exception);
        void LogInformation(string message, object details);
    }
}
