using MercerStore.Web.Application.Interfaces.Repositories;
using MercerStore.Web.Application.Models.sales;
using MercerStore.Web.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace MercerStore.Web.Infrastructure.Repositories
{
    public class SaleRepository : ISaleRepository
    {
        private readonly AppDbContext _context;

        public SaleRepository(AppDbContext context)
        {
            _context = context;
        }


        public async Task AddOfflineSaleItems(OfflineSaleItem saleItem)
        {
            await _context.OfflineSaleItems.AddAsync(saleItem);
            await _context.SaveChangesAsync();
        }
        public async Task AddOfflineSales(OfflineSale sale)
        {
            await _context.OfflineSales.AddAsync(sale);
            await _context.SaveChangesAsync();
        }

        public async Task<OfflineSale?> GetSaleById(int saleId)
        {
            return await _context.OfflineSales
                .Include(o => o.Items)
                .FirstOrDefaultAsync(x => x.Id == saleId);
        }

        public async Task<OfflineSale?> GetSaleByManagerId(string? managerId)
        {
            return await _context.OfflineSales
                .Include(o => o.Items)
                .FirstOrDefaultAsync(x => x.ManagerId == managerId && !x.IsClosed);
        }

        public async Task UpdateSale(OfflineSale sale)
        {
            _context.OfflineSales.Update(sale);
            await _context.SaveChangesAsync();
        }
    }
}
