namespace MercerStore.Interfaces
{
    public interface IUserActivityRepository
    {
        Task UpdateLastActivityAsync(string userId, DateTime lastActivity);
    }
}
