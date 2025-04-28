using MercerStore.Web.Application.Dtos.Metric;
using MercerStore.Web.Application.Interfaces.Repositories;
using MercerStore.Web.Application.Models.Invoices;
using MercerStore.Web.Application.Requests.Suppliers;
using MercerStore.Web.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace MercerStore.Web.Infrastructure.Repositories;

public class SupplierRepository : ISupplierRepository
{
    private readonly AppDbContext _context;

    public SupplierRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Supplier> AddSupplier(Supplier supplier, CancellationToken ct)
    {
        await _context.Suppliers.AddAsync(supplier, ct);
        await _context.SaveChangesAsync(ct);
        return supplier;
    }

    public async Task<(IEnumerable<Supplier>, int totalItems)> GetFilteredSuppliers(SupplierFilterRequest request,
        CancellationToken ct)
    {
        var suppliersQuery = _context.Suppliers
            .Include(s => s.Invoices)
            .AsQueryable();

        if (!string.IsNullOrEmpty(request.Query))
            suppliersQuery = suppliersQuery.Where(u =>
                EF.Functions.ILike(u.Email, $"%{request.Query}%") ||
                EF.Functions.ILike(u.Address, $"%{request.Query}%") ||
                EF.Functions.ILike(u.Phone, $"%{request.Query}%") ||
                EF.Functions.ILike(u.TaxId, $"%{request.Query}%") ||
                EF.Functions.ILike(u.ContactPerson, $"%{request.Query}%")
            );

        var totalItems = await suppliersQuery.CountAsync(ct);

        var suppliers = await suppliersQuery
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(ct);

        return (suppliers, totalItems);
    }

    public async Task<Supplier> GetSupplierById(int supplierId, CancellationToken ct)
    {
        return await _context.Suppliers.FirstOrDefaultAsync(s => s.Id == supplierId, ct);
    }

    public async Task RemoveSupplier(int supplierId, CancellationToken ct)
    {
        var supplier = await _context.Suppliers.FirstOrDefaultAsync(s => s.Id == supplierId, ct);
        _context.Remove(supplier);
        await _context.SaveChangesAsync(ct);
    }

    public async Task<int> UpdateSupplier(Supplier supplier, CancellationToken ct)
    {
        _context.Suppliers.Update(supplier);
        await _context.SaveChangesAsync(ct);
        return supplier.Id;
    }

    public async Task<SupplierMetricDto> GetSupplierMetric(CancellationToken ct)
    {
        var supplierMetrics = new SupplierMetricDto
        {
            Total = await _context.Suppliers
                .AsNoTracking()
                .CountAsync(ct)
        };
        return supplierMetrics;
    }
}
