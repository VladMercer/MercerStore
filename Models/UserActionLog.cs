namespace MercerStore.Models
{
    public class UserActionLog
    {
        public IEnumerable<string>? Roles { get; set; }
        public string? UserId { get; set; }
        public string Action { get; set; }
        public string EntityName { get; set; }
        public int? EntityId { get; set; }
        public object? Details { get; set; }
    }
}
