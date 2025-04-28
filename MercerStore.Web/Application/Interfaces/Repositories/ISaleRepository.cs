using MercerStore.Web.Application.Models.sales;

namespace MercerStore.Web.Application.Interfaces.Repositories;

public interface ISaleRepository
{
    Task<OfflineSale?> GetSaleByManagerId(string? managerId, CancellationToken ct);
    Task<OfflineSale?> GetSaleById(int saleId, CancellationToken ct);
    Task AddOfflineSales(OfflineSale sale, CancellationToken ct);
    Task AddOfflineSaleItems(OfflineSaleItem saleItem, CancellationToken ct);
    Task UpdateSale(OfflineSale sale, CancellationToken ct);
}
