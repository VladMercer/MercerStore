using MercerStore.Web.Application.Dtos.Result;
using MercerStore.Web.Application.Dtos.Supplier;
using MercerStore.Web.Application.Requests.Suppliers;
using MercerStore.Web.Areas.Admin.ViewModels.Suppliers;
using MercerStore.Web.Infrastructure.Helpers;

namespace MercerStore.Web.Application.Interfaces.Services;

public interface ISupplierService
{
    Task<PaginatedResultDto<AdminSupplierDto>> GetFilteredSuppliersWithoutCache(SupplierFilterRequest request,
        CancellationToken ct);

    Task RemoveSupplier(int supplierId, CancellationToken ct);
    Task<int> CreateSupplier(CreateSupplierViewModel createSupplierViewModel, CancellationToken ct);
    Task<UpdateSupplierViewModel> GetUpdateSupplierViewModel(int supplierId, CancellationToken ct);
    Task<Result<int>> UpdateSupplier(UpdateSupplierViewModel updateSupplierViewModel, CancellationToken ct);
}
