using MercerStore.Web.Application.Models.sales;
using MercerStore.Web.Application.Requests.Sales;
using MercerStore.Web.Infrastructure.Helpers;

namespace MercerStore.Web.Application.Interfaces.Services;

public interface ISaleService
{
    Task<OfflineSale> CreateOfflineSale(string managerId, CancellationToken ct);
    Task<Result<int>> AddItem(SaleRequest request, CancellationToken ct);
    Task<Result<int>> CloseSale(int saleId, CancellationToken ct);
    Task<Result<OfflineSale>> GetSummarySale(int saleId, CancellationToken ct);
}