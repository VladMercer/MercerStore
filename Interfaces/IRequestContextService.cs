namespace MercerStore.Interfaces
{
    public interface IRequestContextService
    {
        void SetLogDetails(object logDetails);
        object? GetLogDetails();
    }
}
