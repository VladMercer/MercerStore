namespace MercerStore.Web.Application.Interfaces.Services;

public interface IRequestContextService
{
    void SetLogDetails(object logDetails);
    object? GetLogDetails();
}
