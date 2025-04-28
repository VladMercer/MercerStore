using MediatR;
using MercerStore.Web.Application.Interfaces.Services;

namespace MercerStore.Web.Application.Handlers.Products.Commands;

public record UpdateSkusCommand : IRequest<Unit>;

public class UpdateSkusHandler : IRequestHandler<UpdateSkusCommand, Unit>
{
    private readonly ISkuUpdater _skuUpdater;

    public UpdateSkusHandler(ISkuUpdater SkuUpdater)
    {
        _skuUpdater = SkuUpdater;
    }

    public async Task<Unit> Handle(UpdateSkusCommand request, CancellationToken ct)
    {
        _skuUpdater.UpdateSkus();
        return Unit.Value;
    }
}