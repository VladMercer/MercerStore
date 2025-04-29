using MediatR;
using MercerStore.Web.Application.Interfaces.Services;
using MercerStore.Web.Application.Requests.Log;
using MercerStore.Web.Areas.Admin.ViewModels.Suppliers;

namespace MercerStore.Web.Application.Handlers.Suppliers.Commands;

public record CreateSupplierCommand(CreateSupplierViewModel CreateSupplierViewModel) :
    LoggableRequest<Unit>("Manager create new supplier", "Supplier");

public class CreateSupplierHandler : IRequestHandler<CreateSupplierCommand, Unit>
{
    private readonly ISupplierService _supplierService;

    public CreateSupplierHandler(ISupplierService supplierService)
    {
        _supplierService = supplierService;
    }

    public async Task<Unit> Handle(CreateSupplierCommand request, CancellationToken ct)
    {
        var supplierId = await _supplierService.CreateSupplier(request.CreateSupplierViewModel, ct);

        var createSupplierViewModel = request.CreateSupplierViewModel;

        var logDetails = new
        {
            createSupplierViewModel.Name,
            createSupplierViewModel.Address,
            createSupplierViewModel.Phone,
            createSupplierViewModel.ContactPerson,
            createSupplierViewModel.Email,
            createSupplierViewModel.IsCompany,
            createSupplierViewModel.TaxId
        };

        request.EntityId = supplierId;
        request.Details = logDetails;

        return Unit.Value;
    }
}
