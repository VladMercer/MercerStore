using MercerStore.Dtos.SupplierDto;
using MercerStore.Models.Invoice;

namespace MercerStore.Interfaces
{
    public interface ISupplierRepository
    {
        Task<Supplier> AddSupplier(Supplier supplier);
        Task UpdateSupplier(Supplier supplier);
        Task RemoveSupplier(int supplierId);
        Task<(IEnumerable<AdminSupplierDto>, int totalItems)> GetFilteredSuppliers(
         int pageNumber,
         int pageSize,
         string? query);
        Task<Supplier> GetSupplierById(int supplierId);
        Task<object> GetSupplierMetric();
    }
}
