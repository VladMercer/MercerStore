namespace MercerStore.Web.Application.Interfaces.Services;

public interface ILogService
{
    void LogUserAction<TEntityId>(IEnumerable<string>? roles, string? userId, string action, string entityName,
        TEntityId? entityId, object? details);

    void LogError(string message, Exception exception);
    void LogInformation(string message, object details);
}