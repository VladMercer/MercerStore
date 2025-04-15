using MercerStore.Web.Application.Dtos.Account;
using MercerStore.Web.Application.ViewModels.Users;
using MercerStore.Web.Infrastructure.Helpers;

namespace MercerStore.Web.Application.Interfaces.Services
{
    public interface IAccountService
    {
        Task<Result<JwtTokenDto>> LoginAsync(LoginViewModel loginViewModel);
        Task<Result<JwtTokenDto>> RegisterUserAsync(RegisterViewModel registerViewModel);
        Task<Result<JwtTokenDto>> RegisterManagerAsync(RegisterViewModel registerViewModel);
    }
}
