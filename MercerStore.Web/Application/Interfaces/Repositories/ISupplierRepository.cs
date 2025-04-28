using MercerStore.Web.Application.Dtos.Metric;
using MercerStore.Web.Application.Models.Invoices;
using MercerStore.Web.Application.Requests.Suppliers;

namespace MercerStore.Web.Application.Interfaces.Repositories;

public interface ISupplierRepository
{
    Task<Supplier> AddSupplier(Supplier supplier, CancellationToken ct);
    Task<int> UpdateSupplier(Supplier supplier, CancellationToken ct);
    Task RemoveSupplier(int supplierId, CancellationToken ct);

    Task<(IEnumerable<Supplier>, int totalItems)> GetFilteredSuppliers(SupplierFilterRequest request,
        CancellationToken ct);

    Task<Supplier> GetSupplierById(int supplierId, CancellationToken ct);
    Task<SupplierMetricDto> GetSupplierMetric(CancellationToken ct);
}
