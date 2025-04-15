using MediatR;
using MercerStore.Web.Application.Interfaces;

namespace MercerStore.Web.Application.Handlers.Products.Commands
{
    public record UpdateSkusCommand() : IRequest<Unit>;
    public class UpdateSkusHandler : IRequestHandler<UpdateSkusCommand, Unit>
    {
        private readonly ISkuUpdater _skuUpdater;

        public UpdateSkusHandler(ISkuUpdater skuUpdater)
        {
            _skuUpdater = skuUpdater;
        }

        public async Task<Unit> Handle(UpdateSkusCommand request, CancellationToken cancellationToken)
        {
            _skuUpdater.UpdateSKUs();
            return Unit.Value;
        }
    }
}
