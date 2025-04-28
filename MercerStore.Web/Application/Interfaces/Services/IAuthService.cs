using MercerStore.Web.Application.Requests.Account;

namespace MercerStore.Web.Application.Interfaces.Services;

public interface IAuthService
{
    Task<(string, string)> GenerateGuestToken(GenerateTokenRequest request);
}
