using MercerStore.Web.Application.Dtos.ResultDto;
using MercerStore.Web.Application.Dtos.SupplierDto;
using MercerStore.Web.Application.Requests.Suppliers;
using MercerStore.Web.Areas.Admin.ViewModels.Suppliers;
using MercerStore.Web.Infrastructure.Helpers;

namespace MercerStore.Web.Application.Interfaces.Services
{
    public interface ISupplierService
    {
        Task<PaginatedResultDto<AdminSupplierDto>> GetFilteredSuppliers(SupplierFilterRequest request);
        Task RemoveSupplier(int supplierId);
        Task<int> CreateSupplier(CreateSupplierViewModel createSupplierViewModel);
        Task<UpdateSupplierViewModel> GetUpdateSupplierViewModel(int supplierId);
        Task<Result<int>> UpdateSupplier(UpdateSupplierViewModel updateSupplierViewModel);
    }
}
