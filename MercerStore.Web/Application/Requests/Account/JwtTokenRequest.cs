namespace MercerStore.Web.Application.Requests.Account
{
    public record JwtTokenRequest(
                string UserId,
                List<string> Roles,
                string? ProfilePictureUrl,
                DateTime CreationDate);
}
