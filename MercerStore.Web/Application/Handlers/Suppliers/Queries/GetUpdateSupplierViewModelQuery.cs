using MediatR;
using MercerStore.Web.Application.Interfaces.Services;
using MercerStore.Web.Areas.Admin.ViewModels.Suppliers;

namespace MercerStore.Web.Application.Handlers.Suppliers.Queries;

public record GetUpdateSupplierViewModelQuery(int SupplierId) : IRequest<UpdateSupplierViewModel>;

public class
    GetUpdateSupplierViewModelHandler : IRequestHandler<GetUpdateSupplierViewModelQuery, UpdateSupplierViewModel>
{
    private readonly ISupplierService _supplierService;

    public GetUpdateSupplierViewModelHandler(ISupplierService supplierService)
    {
        _supplierService = supplierService;
    }

    public async Task<UpdateSupplierViewModel> Handle(GetUpdateSupplierViewModelQuery request,
        CancellationToken ct)
    {
        return await _supplierService.GetUpdateSupplierViewModel(request.SupplierId, ct);
    }
}
