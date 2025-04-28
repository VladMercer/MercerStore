using MercerStore.Web.Application.ViewModels.Products;

namespace MercerStore.Web.Application.Interfaces.Services;

public interface IHomeService
{
    Task<HomePageViewModel> GetHomePageProduct(CancellationToken ct);
}
