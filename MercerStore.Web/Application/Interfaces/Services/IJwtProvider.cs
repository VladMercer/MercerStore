using MercerStore.Web.Application.Requests.Account;

namespace MercerStore.Web.Application.Interfaces.Services;

public interface IJwtProvider
{
    Task<string> GenerateJwtToken(JwtTokenRequest request);
}