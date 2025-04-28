namespace MercerStore.Web.Application.Requests.Account;

public record JwtTokenRequest(
    string UserId,
    IList<string> Roles,
    string? ProfilePictureUrl,
    DateTime CreationDate,
    string? TimeZone);