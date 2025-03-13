using MercerStore.Web.Application.Dtos.MetricDto;
using MercerStore.Web.Application.Dtos.SupplierDto;
using MercerStore.Web.Application.Models.Invoice;
using MercerStore.Web.Application.Requests.Suppliers;

namespace MercerStore.Web.Application.Interfaces.Repositories
{
    public interface ISupplierRepository
    {
        Task<Supplier> AddSupplier(Supplier supplier);
        Task<int> UpdateSupplier(Supplier supplier);
        Task RemoveSupplier(int supplierId);
        Task<(IEnumerable<Supplier>, int totalItems)> GetFilteredSuppliers(SupplierFilterRequest request);
        Task<Supplier> GetSupplierById(int supplierId);
        Task<SupplierMetricDto> GetSupplierMetric();
    }
}
