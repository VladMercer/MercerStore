using MercerStore.Web.Application.Dtos.Metric;
using MercerStore.Web.Application.Interfaces.Repositories;
using MercerStore.Web.Application.Models.Invoices;
using MercerStore.Web.Application.Requests.Invoices;
using MercerStore.Web.Areas.Admin.ViewModels.Invoices;
using MercerStore.Web.Infrastructure.Data;
using MercerStore.Web.Infrastructure.Data.Enum;
using MercerStore.Web.Infrastructure.Data.Enum.Invoice;
using Microsoft.EntityFrameworkCore;

namespace MercerStore.Web.Infrastructure.Repositories;

public class InvoiceRepository : IInvoiceRepository
{
    private readonly AppDbContext _context;

    public InvoiceRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddInvoiceItems(IEnumerable<InvoiceItem> invoiceItems, CancellationToken ct)
    {
        await _context.InvoiceItems.AddRangeAsync(invoiceItems, ct);
        await _context.SaveChangesAsync(ct);
    }

    public async Task AddInvoiceItem(InvoiceItem invoiceItem, CancellationToken ct)
    {
        await _context.InvoiceItems.AddAsync(invoiceItem, ct);
        await _context.SaveChangesAsync(ct);
    }

    public async Task AddInvoice(Invoice invoice, CancellationToken ct)
    {
        await _context.Invoices.AddAsync(invoice, ct);
        await _context.SaveChangesAsync(ct);
    }

    public async Task<Invoice?> GetInvoiceById(int invoiceId, CancellationToken ct)
    {
        return await _context.Invoices
            .Include(i => i.InvoiceItems)
            .ThenInclude(i => i.Product)
            .Include(i => i.Supplier)
            .FirstOrDefaultAsync(x => x.Id == invoiceId, ct);
    }

    public async Task<Invoice?> GetInvoiceByManagerId(string? managerId, CancellationToken ct)
    {
        return await _context.Invoices
            .Include(i => i.InvoiceItems)
            .FirstOrDefaultAsync(x => x.ManagerId == managerId && x.Status == InvoiceStatus.Active, ct);
    }

    public async Task UpdateInvoice(Invoice invoice, CancellationToken ct)
    {
        _context.Invoices.Update(invoice);
        await _context.SaveChangesAsync(ct);
    }

    public async Task<(IEnumerable<Invoice>, int totalItems)> GetFilteredInvoices(InvoiceFilterRequest request,
        CancellationToken ct)
    {
        var currentDay = DateTime.UtcNow;
        var invoicesQuery = _context.Invoices
            .Include(r => r.Supplier)
            .AsQueryable();

        if (!string.IsNullOrEmpty(request.Query))
            invoicesQuery = invoicesQuery.Where(u =>
                u.ManagerId == request.Query ||
                EF.Functions.ILike(u.Supplier.Name, $"%{request.Query}%") ||
                EF.Functions.ILike(u.Supplier.Email, $"%{request.Query}%") ||
                EF.Functions.ILike(u.Supplier.Phone, $"%{request.Query}%")
            );

        if (request.Period.HasValue && request.Period != TimePeriod.All)
        {
            var filterDate = request.Period switch
            {
                TimePeriod.Day => currentDay.AddDays(-1),
                TimePeriod.Week => currentDay.AddDays(-7),
                TimePeriod.Month => currentDay.AddMonths(-1),
                TimePeriod.Quarter => currentDay.AddMonths(-3),
                TimePeriod.Year => currentDay.AddYears(-1),
                _ => currentDay
            };

            invoicesQuery = request.Filter switch
            {
                InvoiceFilter.DateReceived => invoicesQuery.Where(o => o.DateReceived >= filterDate),
                InvoiceFilter.EditDate => invoicesQuery.Where(o => o.EditDate >= filterDate),
                InvoiceFilter.PaymentDate => invoicesQuery.Where(o => o.PaymentDate >= filterDate),
                _ => invoicesQuery
            };
        }

        invoicesQuery = request.Filter switch
        {
            InvoiceFilter.Pending => invoicesQuery.Where(u => u.Status == InvoiceStatus.Pending),
            InvoiceFilter.Received => invoicesQuery.Where(u => u.Status == InvoiceStatus.Received),
            InvoiceFilter.PartiallyReceived => invoicesQuery.Where(u => u.Status == InvoiceStatus.PartiallyReceived),
            InvoiceFilter.Rejected => invoicesQuery.Where(u => u.Status == InvoiceStatus.Rejected),
            InvoiceFilter.Closed => invoicesQuery.Where(u => u.Status == InvoiceStatus.Closed),
            _ => invoicesQuery
        };

        invoicesQuery = request.SortOrder switch
        {
            InvoiceSortOrder.DateTimeDesc => invoicesQuery.OrderByDescending(p =>
                p.EditDate ?? p.PaymentDate ?? p.DateReceived),
            InvoiceSortOrder.DateTimeAsc => invoicesQuery.OrderBy(p => p.EditDate ?? p.PaymentDate ?? p.DateReceived),
            InvoiceSortOrder.StatusAsc => invoicesQuery.OrderBy(p => p.Status),
            InvoiceSortOrder.StatusDesc => invoicesQuery.OrderByDescending(p => p.Status),
            InvoiceSortOrder.TotalAmountAsc => invoicesQuery.OrderBy(p => p.TotalAmount),
            InvoiceSortOrder.TotalAmountDesc => invoicesQuery.OrderByDescending(p => p.TotalAmount),
            _ => invoicesQuery.OrderByDescending(p => p.EditDate ?? p.PaymentDate ?? p.DateReceived)
        };

        var totalItems = await invoicesQuery.CountAsync(ct);

        var invoices = await invoicesQuery
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(ct);

        return (invoices, totalItems);
    }

    public async Task UpdateInvoiceItems(IEnumerable<InvoiceItemViewModel> invoiceItems,
        CancellationToken ct)
    {
        foreach (var viewModel in invoiceItems)
        {
            var entity = await _context.InvoiceItems.FirstOrDefaultAsync(x => x.Id == viewModel.Id, ct);

            if (entity != null)
            {
                entity.Quantity = viewModel.Quantity;
                entity.PurchasePrice = viewModel.PurchasePrice;
            }
        }

        await _context.SaveChangesAsync(ct);
    }

    public async Task<InvoiceMetricDto> GetInvoiceMetric(CancellationToken ct)
    {
        var now = DateTime.UtcNow;
        var dayOfWeek = (int)now.DayOfWeek;
        dayOfWeek = dayOfWeek == 0 ? 7 : dayOfWeek;
        var startOfWeek = now.Date.AddDays(1 - dayOfWeek);
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
            .ToListAsync(ct);


        var averageInvoiceValue = await _context.Invoices
            .AsNoTracking()
            .AverageAsync(o => o.TotalAmount, ct);

        var topProducts = await _context.InvoiceItems
            .AsNoTracking()
            .GroupBy(p => p.Product.Name)
            .Select(g => new TopProductDto
            {
                Name = g.Key,
                Sales = g.Sum(x => x.Quantity)
            })
            .OrderByDescending(x => x.Sales)
            .Take(5)
            .ToListAsync(ct);


        var invoicesMetric = new InvoiceMetricDto
        {
            Daily = invoices.Where(x => x.Date == now.Date).Sum(x => x.Amount),
            Weekly = invoices.Where(x => x.Date >= startOfWeek).Sum(x => x.Amount),
            Monthly = invoices.Where(x => x.Date >= startOfMonth).Sum(x => x.Amount),
            Yearly = invoices.Where(x => x.Date >= startOfYear).Sum(x => x.Amount),

            TotalInvoices = new TotalInvoicesDto
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