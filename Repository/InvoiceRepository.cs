using MercerStore.Areas.Admin.ViewModels;
using MercerStore.Data;
using MercerStore.Data.Enum;
using MercerStore.Data.Enum.Invoice;
using MercerStore.Dtos.Invoice;
using MercerStore.Interfaces;
using MercerStore.Models.Invoice;
using Microsoft.EntityFrameworkCore;

namespace MercerStore.Repository
{
    public class InvoiceRepository : IInvoiceRepository
    {
        private readonly AppDbContext _context;

        public InvoiceRepository(AppDbContext context)
        {
            _context = context;
        }


        public async Task AddInvoiceItems(InvoiceItem invoiceItem)
        {
            await _context.InvoiceItems.AddAsync(invoiceItem);
            await _context.SaveChangesAsync();
        }
        public async Task AddInvoice(Invoice invoice)
        {
            await _context.Invoices.AddAsync(invoice);
            await _context.SaveChangesAsync();
        }

        public async Task<Invoice?> GetInvoiceById(int invoiceId)
        {
            return await _context.Invoices
                .Include(i => i.InvoiceItems)
                .ThenInclude(i => i.Product)
                .Include(i => i.Supplier)
                .FirstOrDefaultAsync(x => x.Id == invoiceId);
        }

        public async Task<Invoice?> GetInvoiceByManagerId(string? managerId)
        {
            return await _context.Invoices
                .Include(i => i.InvoiceItems)
                .FirstOrDefaultAsync(x => x.ManagerId == managerId && (x.Status == InvoiceStatus.activ));
        }

        public async Task UpdateInvoice(Invoice invoice)
        {
            _context.Invoices.Update(invoice);
            await _context.SaveChangesAsync();
        }
        public async Task<(IEnumerable<InvoiceDto>, int totalItems)> GetFilteredInvoices(
             int pageNumber,
             int pageSize,
             InvoiceSortOrder? sortOrder,
             TimePeriod? timePeriod,
             InvoiceFilter? filter,
             string? query)
        {

            var currentDay = DateTime.UtcNow;
            var invoicesQuery = _context.Invoices
                .Include(r => r.Supplier)
                .AsQueryable();

            if (!string.IsNullOrEmpty(query))
            {
                invoicesQuery = invoicesQuery.Where(u =>
                    u.ManagerId == query ||
                    EF.Functions.ILike(u.Supplier.Name, $"%{query}%") ||
                    EF.Functions.ILike(u.Supplier.Email, $"%{query}%") ||
                    EF.Functions.ILike(u.Supplier.Phone, $"%{query}%")
                );
            }

            if (timePeriod.HasValue && timePeriod != TimePeriod.All)
            {
                var filterDate = timePeriod switch
                {
                    TimePeriod.Day => currentDay.AddDays(-1),
                    TimePeriod.Week => currentDay.AddDays(-7),
                    TimePeriod.Month => currentDay.AddMonths(-1),
                    TimePeriod.Quarter => currentDay.AddMonths(-3),
                    TimePeriod.Year => currentDay.AddYears(-1),
                    _ => currentDay,
                };

                invoicesQuery = filter switch
                {
                    InvoiceFilter.DateReceived => invoicesQuery.Where(o => o.DateReceived >= filterDate),
                    InvoiceFilter.EditDate => invoicesQuery.Where(o => o.EditDate >= filterDate),
                    InvoiceFilter.PaymentDate => invoicesQuery.Where(o => o.PaymentDate >= filterDate),
                    _ => invoicesQuery,
                };
            }

            invoicesQuery = filter switch
            {
                InvoiceFilter.Pending => invoicesQuery.Where(u => u.Status == InvoiceStatus.Pending),
                InvoiceFilter.Received => invoicesQuery.Where(u => u.Status == InvoiceStatus.Received),
                InvoiceFilter.PartiallyReceived => invoicesQuery.Where(u => u.Status == InvoiceStatus.PartiallyReceived),
                InvoiceFilter.Rejected => invoicesQuery.Where(u => u.Status == InvoiceStatus.Rejected),
                InvoiceFilter.Closed => invoicesQuery.Where(u => u.Status == InvoiceStatus.Closed),
                _ => invoicesQuery,
            };

            invoicesQuery = sortOrder switch
            {
                InvoiceSortOrder.DateTimeDesc => invoicesQuery.OrderByDescending(p => p.EditDate ?? p.PaymentDate ?? p.DateReceived),
                InvoiceSortOrder.DateTimeAsc => invoicesQuery.OrderBy(p => p.EditDate ?? p.PaymentDate ?? p.DateReceived),
                InvoiceSortOrder.StatusAsc => invoicesQuery.OrderBy(p => p.Status),
                InvoiceSortOrder.StatusDesc => invoicesQuery.OrderByDescending(p => p.Status),
                InvoiceSortOrder.TotalAmountAsc => invoicesQuery.OrderBy(p => p.TotalAmount),
                InvoiceSortOrder.TotalAmountDesc => invoicesQuery.OrderByDescending(p => p.TotalAmount),
                _ => invoicesQuery.OrderByDescending(p => p.EditDate ?? p.PaymentDate ?? p.DateReceived)
            };

            var totalItems = await invoicesQuery.CountAsync();

            var invoices = await invoicesQuery
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var invoiceDtos = invoices.Select(i => new InvoiceDto
            {
                Id = i.Id,
                ManagerId = i.ManagerId,
                SupplierId = i.SupplierId,
                Status = i.Status switch
                {
                    InvoiceStatus.Pending => "Ожидается",
                    InvoiceStatus.Received => "Получена",
                    InvoiceStatus.PartiallyReceived => "Частично получена",
                    InvoiceStatus.Rejected => "Отклноненна",
                    InvoiceStatus.Closed => "Закрыта",
                    _ => "Неизвестный статус"
                },
                CompanyName = i.Supplier.Name,
                DateReceived = i.DateReceived,
                PaymentDate = i.PaymentDate,
                TotalAmount = i.TotalAmount,
                EditDate = i.EditDate,
                Email = i.Supplier.Email,
                Phone = i.Supplier.Phone,

            }).ToList();

            return (invoiceDtos, totalItems);
        }
        public async Task UpdateInvoiceItems(List<InvoiceItemViewModel> invoiceItems)
        {
            foreach (var viewModel in invoiceItems)
            {
                var entity = await _context.InvoiceItems.FirstOrDefaultAsync(x => x.Id == viewModel.Id);

                if (entity != null)
                {
                    entity.Quantity = viewModel.Quantity;
                    entity.PurchasePrice = viewModel.PurchasePrice;
                }
            }

            await _context.SaveChangesAsync();
        }
        public async Task<object> GetInvoiceMetric()
        {
            var now = DateTime.UtcNow;
            var startOfWeek = now.Date.AddDays(-(int)now.DayOfWeek + 1);
            var startOfMonth = new DateTime(now.Year, now.Month, 1, 0, 0, 0, DateTimeKind.Utc);
            var startOfYear = new DateTime(now.Year, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            
            var invoices = await _context.Invoices
                .AsNoTracking()
                .Where(o => o.Status == InvoiceStatus.Closed && o.PaymentDate.HasValue)
                .GroupBy(o => o.PaymentDate.Value.Date)
                .Select(g => new
                {
                    Date = g.Key,
                    Amount = g.Sum(o => o.TotalAmount),
                    Invoices = g.Count()
                })
                .ToListAsync();

           
            var averageInvoiceValue = await _context.Invoices
                .AsNoTracking()
                .AverageAsync(o => o.TotalAmount);

            var topProducts = await _context.InvoiceItems
                .AsNoTracking()
                .GroupBy(p => p.Product.Name)
                .Select(g => new
                {
                    Name = g.Key,
                    Invoices = g.Sum(x => x.Quantity)
                })
                .OrderByDescending(x => x.Invoices)
                .Take(5)
                .ToListAsync();

        
            var invoicesMetric = new
            {
                Daily = invoices.Where(x => x.Date == now.Date).Sum(x => x.Amount),
                Weekly = invoices.Where(x => x.Date >= startOfWeek).Sum(x => x.Amount),
                Monthly = invoices.Where(x => x.Date >= startOfMonth).Sum(x => x.Amount),
                Yearly = invoices.Where(x => x.Date >= startOfYear).Sum(x => x.Amount),

                TotalInvoices = new
                {
                    Daily = invoices.Where(x => x.Date == now.Date).Sum(x => x.Invoices),
                    Weekly = invoices.Where(x => x.Date >= startOfWeek).Sum(x => x.Invoices),
                    Monthly = invoices.Where(x => x.Date >= startOfMonth).Sum(x => x.Invoices),
                    Yearly = invoices.Where(x => x.Date >= startOfYear).Sum(x => x.Invoices)
                },

                AverageInvoiceValue = averageInvoiceValue,
                TopProducts = topProducts
            };

            return invoicesMetric;
        }
    }
}
