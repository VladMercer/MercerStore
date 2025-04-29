namespace MercerStore.Web.Application.Interfaces.Services;

public interface IUserActivityService
{
    Task UpdateUserActivity(CancellationToken ct);
}
