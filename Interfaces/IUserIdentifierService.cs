namespace MercerStore.Interfaces
{
    public interface IUserIdentifierService
    {
        string GetCurrentIdentifier();
        string GetCurrentUserRoles();

	}
}
