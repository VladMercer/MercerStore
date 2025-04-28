namespace MercerStore.Web.Application.Interfaces.Repositories;

public interface IUserActivityRepository
{
    Task UpdateLastActivityAsync(string userId, DateTime lastActivity, CancellationToken ct);
}