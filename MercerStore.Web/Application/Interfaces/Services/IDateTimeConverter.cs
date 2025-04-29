namespace MercerStore.Web.Application.Interfaces.Services;

public interface IDateTimeConverter
{
    DateTime ConvertUtcToUserTime(DateTime utcTime);
    DateTime ConvertUserTimeToUtc(DateTime userTime);
}
