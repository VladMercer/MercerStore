using MercerStore.Web.Application.ViewModels.Users;
using MercerStore.Web.Infrastructure.Helpers;

namespace MercerStore.Web.Application.Interfaces.Services
{
    public interface IAccountService
    {
        Task<Result<string>> LoginAsync(LoginViewModel loginViewModel, string ipAddress);
        Task<Result<string>> RegisterUserAsync(RegisterViewModel model, string ipAddress);
        Task<Result<string>> RegisterManagerAsync(RegisterViewModel model, string ipAddress);
    }
}
