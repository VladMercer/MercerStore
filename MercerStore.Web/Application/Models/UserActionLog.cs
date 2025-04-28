namespace MercerStore.Web.Application.Models;

public class UserActionLog<TEntityId>
{
    public IEnumerable<string>? Roles { get; set; }
    public string? UserId { get; set; }
    public string Action { get; set; }
    public string EntityName { get; set; }
    public TEntityId? EntityId { get; set; }
    public object? Details { get; set; }
}