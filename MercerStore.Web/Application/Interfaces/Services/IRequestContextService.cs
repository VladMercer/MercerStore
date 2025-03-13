namespace MercerStore.Web.Application.Interfaces
{
    public interface IRequestContextService
    {
        void SetLogDetails(object logDetails);
        object? GetLogDetails();
    }
}
