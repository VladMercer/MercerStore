namespace MercerStore.Web.Application.Interfaces
{
    public interface IJwtProvider
    {
        string GenerateJwtToken(string userId, IEnumerable<string> roles, string? profilePictureUrl, DateTime creationDate);
    }
}
