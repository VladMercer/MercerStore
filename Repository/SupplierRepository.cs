using MercerStore.Data;
using MercerStore.Data.Enum;
using MercerStore.Data.Enum.User;
using MercerStore.Dtos.SupplierDto;
using MercerStore.Dtos.UserDto;
using MercerStore.Interfaces;
using MercerStore.Models.Invoice;
using Microsoft.EntityFrameworkCore;

namespace MercerStore.Repository
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

        public async Task<(IEnumerable<AdminSupplierDto>, int totalItems)> GetFilteredSuppliers(
         int pageNumber,
         int pageSize,
         string? query)
        {

            var suppliersQuery = _context.Suppliers
                .Include(s => s.Invoices)
                .AsQueryable();

            if (!string.IsNullOrEmpty(query))
            {
                suppliersQuery = suppliersQuery.Where(u =>
                    EF.Functions.ILike(u.Email, $"%{query}%") ||
                    EF.Functions.ILike(u.Address, $"%{query}%") ||
                    EF.Functions.ILike(u.Phone, $"%{query}%") ||
                    EF.Functions.ILike(u.TaxId, $"%{query}%") ||
                    EF.Functions.ILike(u.ContactPerson, $"%{query}%")
                );
            }

            var totalItems = await suppliersQuery.CountAsync();

            var suppliers = await suppliersQuery
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var supplierDtos = suppliers.Select(s => new AdminSupplierDto
            {
                Id = s.Id,
                Phone = s.Phone,
                Address = s.Address,
                ContactPerson = s.ContactPerson,
                Email = s.Email,
                IsCompany = s.IsCompany,
                Name = s.Name,
                TaxId = s.TaxId
            }).ToList();

            return (supplierDtos, totalItems);
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

        public async Task UpdateSupplier(Supplier supplier)
        {
            _context.Suppliers.Update(supplier);
            await _context.SaveChangesAsync();
        }
        public async Task<object> GetSupplierMetric()
        {
            var supplierMetrics = new
            {
                Total = await _context.Suppliers
                .AsNoTracking()
                .CountAsync(),
            };
            return supplierMetrics;
        }
    }
}
