using MediatR;
using MercerStore.Web.Application.Interfaces.Services;
using MercerStore.Web.Application.ViewModels.Products;

namespace MercerStore.Web.Application.Handlers.Home.Queries;

public record GetHomePageProductQuery : IRequest<HomePageViewModel>;

public class HomeHandler : IRequestHandler<GetHomePageProductQuery, HomePageViewModel>
{
    private readonly IHomeService _homeService;

    public HomeHandler(IHomeService homeService)
    {
        _homeService = homeService;
    }

    public async Task<HomePageViewModel> Handle(GetHomePageProductQuery request, CancellationToken ct)
    {
        return await _homeService.GetHomePageProduct(ct);
    }
}
