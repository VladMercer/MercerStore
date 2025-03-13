using MercerStore.Web.Application.Models.sales;
using Microsoft.AspNetCore.Mvc;

namespace MercerStore.Web.Application.Interfaces.Repositories
{
    public interface ISaleRepository
    {
        Task<OfflineSale?> GetSaleByManagerId(string? managerId);
        Task<OfflineSale?> GetSaleById(int saleId);
        Task AddOfflineSales(OfflineSale saleI);
        Task AddOfflineSaleItems(OfflineSaleItem saleItem);
        Task UpdateSale(OfflineSale sale);
    }
}
