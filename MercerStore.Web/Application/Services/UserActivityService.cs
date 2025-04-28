using MercerStore.Web.Application.Interfaces.Repositories;
using MercerStore.Web.Application.Interfaces.Services;

namespace MercerStore.Web.Application.Services;

public class UserActivityService : IUserActivityService
{
    private readonly IUserActivityRepository _userActivityRepository;
    private readonly IUserIdentifierService _userIdentifierService;

    public UserActivityService(IUserActivityRepository userActivityRepository,
        IUserIdentifierService userIdentifierService)
    {
        _userActivityRepository = userActivityRepository;
        _userIdentifierService = userIdentifierService;
    }

    public async Task UpdateUserActivity(CancellationToken ct)
    {
        var userId = _userIdentifierService.GetCurrentIdentifier();
        await _userActivityRepository.UpdateLastActivityAsync(userId, DateTime.UtcNow, ct);
    }
}
