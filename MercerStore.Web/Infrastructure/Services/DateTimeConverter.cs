using MercerStore.Web.Application.Interfaces.Services;

namespace MercerStore.Web.Infrastructure.Services;

public class DateTimeConverter : IDateTimeConverter
{
    private readonly IUserIdentifierService _userIdentifierService;

    public DateTimeConverter(IUserIdentifierService userIdentifierService)
    {
        _userIdentifierService = userIdentifierService;
    }

    public DateTime ConvertUtcToUserTime(DateTime utcTime)
    {
        var userTimeZone = _userIdentifierService.GetCurrentUserTimeZone();
        var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(userTimeZone);
        return TimeZoneInfo.ConvertTimeFromUtc(utcTime, timeZoneInfo);
    }

    public DateTime ConvertUserTimeToUtc(DateTime userTime)
    {
        var userTimeZone = _userIdentifierService.GetCurrentUserTimeZone();
        var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(userTimeZone);
        return TimeZoneInfo.ConvertTimeToUtc(userTime, timeZoneInfo);
    }
}