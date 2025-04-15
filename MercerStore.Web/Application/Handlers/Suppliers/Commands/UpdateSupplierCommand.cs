using MediatR;
using MercerStore.Web.Application.Interfaces.Services;
using MercerStore.Web.Application.Requests.Log;
using MercerStore.Web.Areas.Admin.ViewModels.Suppliers;
using MercerStore.Web.Infrastructure.Helpers;

namespace MercerStore.Web.Application.Handlers.Suppliers.Commands
{
    public record UpdateSupplierCommand(UpdateSupplierViewModel UpdateSupplierViewModel) :
        LoggableRequest<Result<Unit>>("Manager update supplier", "supplier");
    public class UpdateSupplierHanlder : IRequestHandler<UpdateSupplierCommand, Result<Unit>>
    {
        private readonly ISupplierService _supplierService;

        public UpdateSupplierHanlder(ISupplierService supplierService)
        {
            _supplierService = supplierService;
        }

        public async Task<Result<Unit>> Handle(UpdateSupplierCommand request, CancellationToken cancellationToken)
        {
            var result = await _supplierService.UpdateSupplier(request.UpdateSupplierViewModel);

            var supplier = request.UpdateSupplierViewModel;
            request.EntityId = supplier.Id;

            if (!result.IsSuccess)
            {
                request.Details = new { result.ErrorMessage };
                return Result<Unit>.Failure(result.ErrorMessage);
            }

            var logDetails = new
            {
                supplier.Address,
                supplier.Name,
                supplier.Phone,
                supplier.Email,
                supplier.ContactPerson,
                supplier.IsCompany,
                supplier.TaxId
            };

            request.Details = logDetails;


            return Result<Unit>.Success(Unit.Value);
        }
    }
}
