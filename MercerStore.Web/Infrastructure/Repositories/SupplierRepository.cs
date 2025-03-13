using MercerStore.Web.Application.Dtos.MetricDto;
using MercerStore.Web.Application.Interfaces.Repositories;
using MercerStore.Web.Application.Models.Invoice;
using MercerStore.Web.Application.Requests.Suppliers;
using MercerStore.Web.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace MercerStore.Web.Infrastructure.Repositories
{
    public class SupplierRepository : ISupplierRepository
    {
        private readonly AppDbContext _context;

        public SupplierRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Supplier> AddSupplier(Supplier supplier)
        {
            await _context.Suppliers.AddAsync(supplier);
            await _context.SaveChangesAsync();
            return supplier;
        }

        public async Task<(IEnumerable<Supplier>, int totalItems)> GetFilteredSuppliers(SupplierFilterRequest request)
        {
            var suppliersQuery = _context.Suppliers
                .Include(s => s.Invoices)
                .AsQueryable();

            if (!string.IsNullOrEmpty(request.Query))
            {
                suppliersQuery = suppliersQuery.Where(u =>
                    EF.Functions.ILike(u.Email, $"%{request.Query}%") ||
                    EF.Functions.ILike(u.Address, $"%{request.Query}%") ||
                    EF.Functions.ILike(u.Phone, $"%{request.Query}%") ||
                    EF.Functions.ILike(u.TaxId, $"%{request.Query}%") ||
                    EF.Functions.ILike(u.ContactPerson, $"%{request.Query}%")
                );
            }

            var totalItems = await suppliersQuery.CountAsync();

            var suppliers = await suppliersQuery
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync();

            return (suppliers, totalItems);
        }

        public async Task<Supplier> GetSupplierById(int supplierId)
        {
            return await _context.Suppliers.FirstOrDefaultAsync(s => s.Id == supplierId);
        }

        public async Task RemoveSupplier(int supplierId)
        {
            var supplier = await _context.Suppliers.FirstOrDefaultAsync(s => s.Id == supplierId);
            _context.Remove(supplier);
            await _context.SaveChangesAsync();
        }

        public async Task<int> UpdateSupplier(Supplier supplier)
        {
            _context.Suppliers.Update(supplier);
            await _context.SaveChangesAsync();
            return supplier.Id;
        }
        public async Task<SupplierMetricDto> GetSupplierMetric()
        {
            var supplierMetrics = new SupplierMetricDto
            {
                Total = await _context.Suppliers
                .AsNoTracking()
                .CountAsync(),
            };
            return supplierMetrics;
        }
    }
}
