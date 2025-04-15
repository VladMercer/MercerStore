using MediatR;
using MercerStore.Web.Application.Interfaces.Services;
using MercerStore.Web.Application.Requests.Log;

namespace MercerStore.Web.Application.Handlers.Suppliers.Commands
{
    public record RemoveSupplierCommand(int SupplierId) :
        LoggableRequest<Unit>("Manager remove supplier", "supplier");
    public class RemoveSupplierHandler : IRequestHandler<RemoveSupplierCommand, Unit>
    {
        private readonly ISupplierService _supplierService;

        public RemoveSupplierHandler(ISupplierService supplierService)
        {
            _supplierService = supplierService;
        }

        public async Task<Unit> Handle(RemoveSupplierCommand request, CancellationToken cancellationToken)
        {
            await _supplierService.RemoveSupplier(request.SupplierId);
            request.EntityId = request.SupplierId;

            return Unit.Value;
        }
    }
}
