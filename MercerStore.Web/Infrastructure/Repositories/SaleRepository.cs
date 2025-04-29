using MercerStore.Web.Application.Interfaces.Repositories;
using MercerStore.Web.Application.Models.sales;
using MercerStore.Web.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace MercerStore.Web.Infrastructure.Repositories;

public class SaleRepository : ISaleRepository
{
    private readonly AppDbContext _context;

    public SaleRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddOfflineSaleItems(OfflineSaleItem saleItem, CancellationToken ct)
    {
        await _context.OfflineSaleItems.AddAsync(saleItem, ct);
        await _context.SaveChangesAsync(ct);
    }

    public async Task AddOfflineSales(OfflineSale sale, CancellationToken ct)
    {
        await _context.OfflineSales.AddAsync(sale, ct);
        await _context.SaveChangesAsync(ct);
    }

    public async Task<OfflineSale?> GetSaleById(int saleId, CancellationToken ct)
    {
        return await _context.OfflineSales
            .Include(o => o.Items)
            .FirstOrDefaultAsync(x => x.Id == saleId, ct);
    }

    public async Task<OfflineSale?> GetSaleByManagerId(string? managerId, CancellationToken ct)
    {
        return await _context.OfflineSales
            .Include(o => o.Items)
            .FirstOrDefaultAsync(x => x.ManagerId == managerId && !x.IsClosed, ct);
    }

    public async Task UpdateSale(OfflineSale sale, CancellationToken ct)
    {
        _context.OfflineSales.Update(sale);
        await _context.SaveChangesAsync(ct);
    }
}
